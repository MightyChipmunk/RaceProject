using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Front,
        Left,
        Right,
    }
    bool isDrift = false;
    public bool IsDrift { get { return isDrift; } }
    bool isBooster = false;
    public bool IsBooster { get { return isBooster; } }

    bool canMove = false;
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    float lerp = 0;
    public float Lerp { get { return lerp; } }
    float origSpeed;
    float origRotSpeed;

    PlayerStat stat;



    PlayerSkill skill;
    Rigidbody rig;
    GameObject cam;

    [SerializeField]
    AudioClip driftClip;
    [SerializeField]
    AudioClip boostClip;
    [SerializeField]
    AudioClip jumpClip;
    [SerializeField]
    AudioClip engineClip;
    [SerializeField]
    AudioClip collideClip;

    [SerializeField]
    bool is2P = false;
    public bool Is2P { get { return is2P; } }

    public PlayerState _state = PlayerState.Front;

    // Start is called before the first frame update
    void Start()
    {
        stat = gameObject.GetComponent<PlayerStat>();
        skill = gameObject.GetComponent<PlayerSkill>();
        rig = gameObject.GetComponent<Rigidbody>();
        if (!is2P)
            cam = GameObject.Find("CameraView");
        else if (is2P)
            cam = GameObject.Find("CameraView_Multi");

        origSpeed = stat.Speed;
        origRotSpeed = stat.RotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !is2P)
            carMove();
        else if (canMove && is2P)
            carMove_Mul();

        
        // 공중에서 차량의 각도를 제한하고 싶다.
        float rx = transform.eulerAngles.z;
        float rz = transform.eulerAngles.x;
        if (rx > 40 && rx < 200)
            rx = 40;
        else if (rx < 320 && rx > 200)
            rx = 320;
        if (rz > 40 && rz < 200)
            rz = 40;
        else if (rz < 320 && rz > 2000)
            rz = 320;
        transform.eulerAngles = new Vector3(rz, transform.eulerAngles.y, rx);

        // 현재 직진인지 좌회전인지 우회전인지 확인
        if (!is2P)
        {
            if (InputManager.Instance.Horizon > 0)
            {
                _state = PlayerState.Right;
            }
            else if (InputManager.Instance.Horizon < 0)
            {
                _state = PlayerState.Left;
            }
            else if (InputManager.Instance.Horizon == 0)
            {
                _state = PlayerState.Front;
            }
        }
        else if (is2P)
        {
            if (InputManager.Instance.Horizon2 > 0)
            {
                _state = PlayerState.Right;
            }
            else if (InputManager.Instance.Horizon2 < 0)
            {
                _state = PlayerState.Left;
            }
            else if (InputManager.Instance.Horizon2 == 0)
            {
                _state = PlayerState.Front;
            }
        }
    }

    // 아래로 Raycast를 쏴서 차량이 공중에 있는지 확인하고 싶다.
    bool IsGrounded() 
    {
        return  Physics.Raycast(transform.position + Vector3.up * 0.5f - Vector3.forward, -Vector3.up, 0.8f, LayerMask.GetMask("Block"))
             || Physics.Raycast(transform.position + Vector3.up * 0.5f - Vector3.forward, -Vector3.up, 0.8f, LayerMask.GetMask("Block"));
    }

    public void carMove()
    {

        // 지상에서 A, D 키를 누를 때 차량 각도를 조정하고싶다.
        if (IsGrounded())
        {
            // 만약 속도가 0 이상이면
            if (lerp >= -0.1f)
                // 버튼을 누르는 방향으로 차량이 회전한다.
                transform.eulerAngles += InputManager.Instance.Horizon * stat.RotSpeed * Time.deltaTime * Vector3.up;
            // 속도가 마이너스라면(후진 중이라면)
            else
                // 버튼을 누르는 반대방향으로 차량이 회전한다.    
                transform.eulerAngles -= InputManager.Instance.Horizon * stat.RotSpeed * Time.deltaTime * Vector3.up;
        }
        // 공중에서 A, D 키를 누를 때 차량을 좌우로 수평이동하고 싶다.
        else
        {
            // 차량 기준이 아닌 카메라가 바라보는 방향을 기준으로 좌우로 이동하고 싶다.
            Vector3 camDir = cam.transform.rotation * Vector3.right;
            transform.position += InputManager.Instance.Horizon * 10 * Time.deltaTime * camDir;
        }

        // 차량이 바라보는 방향으로 전/후진 하고싶다.
        Vector3 dir = transform.rotation * Vector3.forward;
        if (!IsGrounded())
        {
            // 만약 드리프트 중간에 공중에 뜨면 드리프트 중단
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            isDrift = false;
            // 공중에선 차량이 바라보는 방향 중 Y축 방향을 배제하고 이동하고 싶다.
            dir.y = 0;
            // 공중에서 드리프트를 누르면 차량 좌우 방향을 전환하고 싶다.
            if (InputManager.Instance.Drift)
            {
                transform.eulerAngles += InputManager.Instance.Horizon * stat.RotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // 가속도 운동을 구현하고 싶다.
        if (InputManager.Instance.Accel)
        {
            lerp = Mathf.Lerp(lerp, stat.Speed, Time.deltaTime / 10 * stat.AccelPower);
            transform.position += lerp * dir * Time.deltaTime;
            // 직진 시에 엔진 사운드를 재생한다.
            SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
        }
        else if (InputManager.Instance.Brake)
        {
            lerp = Mathf.Lerp(lerp, -stat.Speed / 3, Time.deltaTime / 10 * stat.BrakePower);
            transform.position += lerp * dir * Time.deltaTime;
            // 브레이크를 누르면 엔진 사운드를 서서히 멈춘다.
            if (lerp > 0)
                SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
            // 만약 속도가 0 아래라면 피치 값으로 -(속도) 값을 넘겨준다.
            else
                SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, -lerp / origSpeed * 2);
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime / 5);
            transform.position += lerp * dir * Time.deltaTime;
            // 직진을 안 누르면 엔진 사운드를 서서히 멈춘다.
            SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
        }

        // 체공시간을 늘리고 싶다.
        //rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // 스페이스바를 누를 때 지상에 있으면 점프
        if (InputManager.Instance.Jump)
        {
            if (IsGrounded())
                skill.Jump();
            // 점프 소리 재생
            SoundManager.Instance.Stop(SoundManager.Sound.Drift);
            SoundManager.Instance.Play(jumpClip, SoundManager.Sound.Jump);
        }

        if (!IsGrounded())
            SoundManager.Instance.Stop(SoundManager.Sound.Drift); ;

        // 지상에서 쉬프트키를 누르면 드리프트
        if (IsGrounded())
        {
            if (InputManager.Instance.Drift)
            {
                skill.Drift();
                // 드리프트 소리 재생
                SoundManager.Instance.Play(driftClip, SoundManager.Sound.Drift);
                isDrift = true;
            }
            else if (InputManager.Instance.DriftEnd)
            {
                // 드리프트 키를 떼면 속도를 원상복귀
                stat.Speed = origSpeed;
                stat.RotSpeed = origRotSpeed;
                // 드리프트 소리 멈춤
                SoundManager.Instance.Stop(SoundManager.Sound.Drift);
                isDrift = false;
            }
        }

        // 컨트롤 키를 누르면 부스터
        if (InputManager.Instance.Boost && stat.BoostGauge >= 0)
        {
            skill.Boost();
            // 부스터 소리 재생
            SoundManager.Instance.Play(boostClip, SoundManager.Sound.Booster);
            isBooster = true;
        }
        else if (InputManager.Instance.BoostEnd || stat.BoostGauge < 0)
        {
            // 부스터 키를 떼거나 부스트 게이지가 없으면 속도를 원상복귀
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            // 부스터 소리 멈춤
            SoundManager.Instance.Stop(SoundManager.Sound.Booster);
            isBooster = false;
        }

        // 부스터와 드리프트를 동시에 할 때의 경우를 따로 빼서 처리
        if (isDrift && isBooster)
        {
            stat.Speed = origSpeed;
        }

        
    }

    public void carMove_Mul()
    {

        // 지상에서 A, D 키를 누를 때 차량 각도를 조정하고싶다.
        if (IsGrounded())
        {
            // 만약 속도가 0 이상이면
            if (lerp >= -0.1f)
                // 버튼을 누르는 방향으로 차량이 회전한다.
                transform.eulerAngles += InputManager.Instance.Horizon2 * stat.RotSpeed * Time.deltaTime * Vector3.up;
            // 속도가 마이너스라면(후진 중이라면)
            else
                // 버튼을 누르는 반대방향으로 차량이 회전한다.    
                transform.eulerAngles -= InputManager.Instance.Horizon2 * stat.RotSpeed * Time.deltaTime * Vector3.up;
        }
        // 공중에서 A, D 키를 누를 때 차량을 좌우로 수평이동하고 싶다.
        else
        {
            // 차량 기준이 아닌 카메라가 바라보는 방향을 기준으로 좌우로 이동하고 싶다.
            Vector3 camDir = cam.transform.rotation * Vector3.right;
            transform.position += InputManager.Instance.Horizon2 * 10 * Time.deltaTime * camDir;
        }

        // 차량이 바라보는 방향으로 전/후진 하고싶다.
        Vector3 dir = transform.rotation * Vector3.forward;
        if (!IsGrounded())
        {
            // 만약 드리프트 중간에 공중에 뜨면 드리프트 중단
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            isDrift = false;
            // 공중에선 차량이 바라보는 방향 중 Y축 방향을 배제하고 이동하고 싶다.
            dir.y = 0;
            // 공중에서 드리프트를 누르면 차량 좌우 방향을 전환하고 싶다.
            if (InputManager.Instance.Drift2)
            {
                transform.eulerAngles += InputManager.Instance.Horizon2 * stat.RotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // 가속도 운동을 구현하고 싶다.
        if (InputManager.Instance.Accel2)
        {
            lerp = Mathf.Lerp(lerp, stat.Speed, Time.deltaTime / 10 * stat.AccelPower);
            transform.position += lerp * dir * Time.deltaTime;
            // 직진 시에 엔진 사운드를 재생한다.
            SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, lerp / origSpeed * 2);
        }
        else if (InputManager.Instance.Brake2)
        {
            lerp = Mathf.Lerp(lerp, -stat.Speed / 3, Time.deltaTime / 10 * stat.BrakePower);
            transform.position += lerp * dir * Time.deltaTime;
            // 브레이크를 누르면 엔진 사운드를 서서히 멈춘다.
            if (lerp > 0)
                SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, lerp / origSpeed * 2);
            // 만약 속도가 0 아래라면 피치 값으로 -(속도) 값을 넘겨준다.
            else
                SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, -lerp / origSpeed * 2);
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime / 5);
            transform.position += lerp * dir * Time.deltaTime;
            // 직진을 안 누르면 엔진 사운드를 서서히 멈춘다.
            SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, lerp / origSpeed * 2);
        }

        // 체공시간을 늘리고 싶다.
        //rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // 스페이스바를 누를 때 지상에 있으면 점프
        if (InputManager.Instance.Jump2)
        {
            if (IsGrounded())
                skill.Jump();
            // 점프 소리 재생
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift);
            SoundManager_Multi.Instance.Play(jumpClip, SoundManager_Multi.Sound.Jump);
        }

        if (!IsGrounded())
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift); ;

        // 지상에서 쉬프트키를 누르면 드리프트
        if (IsGrounded())
        {
            if (InputManager.Instance.Drift2)
            {
                skill.Drift();
                // 드리프트 소리 재생
                SoundManager_Multi.Instance.Play(driftClip, SoundManager_Multi.Sound.Drift);
                isDrift = true;
            }
            else if (InputManager.Instance.DriftEnd2)
            {
                // 드리프트 키를 떼면 속도를 원상복귀
                stat.Speed = origSpeed;
                stat.RotSpeed = origRotSpeed;
                // 드리프트 소리 멈춤
                SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift);
                isDrift = false;
            }
        }

        // 컨트롤 키를 누르면 부스터
        if (InputManager.Instance.Boost2 && stat.BoostGauge >= 0)
        {
            skill.Boost();
            // 부스터 소리 재생
            SoundManager_Multi.Instance.Play(boostClip, SoundManager_Multi.Sound.Booster);
            isBooster = true;
        }
        else if (InputManager.Instance.BoostEnd2 || stat.BoostGauge < 0)
        {
            // 부스터 키를 떼거나 부스트 게이지가 없으면 속도를 원상복귀
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            // 부스터 소리 멈춤
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Booster);
            isBooster = false;
        }

        // 부스터와 드리프트를 동시에 할 때의 경우를 따로 빼서 처리
        if (isDrift && isBooster)
        {
            stat.Speed = origSpeed;
        }
    }

    // 충돌했을 때 장애물을 뚫는 오류를 해결하기 위해 작성
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && !is2P)
        {
            // 충돌 시 차량의 속도를 0으로 만든다.
            lerp = 0;
            SoundManager.Instance.Play(collideClip, SoundManager.Sound.Collide);
        }
        else if (collision.gameObject.layer == 9 && is2P)
        {
            // 충돌 시 차량의 속도를 0으로 만든다.
            lerp = 0;
            SoundManager_Multi.Instance.Play(collideClip, SoundManager_Multi.Sound.Collide);
        }

        if (collision.gameObject.name.Contains("Player"))
        {
            SoundManager.Instance.Play(collideClip, SoundManager.Sound.Collide);
        }
    }
}
