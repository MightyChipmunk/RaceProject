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
    public bool isDrift = false;
    public bool isBooster = false;

    float lerp = 0;
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

    public PlayerState _state = PlayerState.Front;

    // Start is called before the first frame update
    void Start()
    {
        stat = gameObject.GetComponent<PlayerStat>();
        skill = gameObject.GetComponent<PlayerSkill>();
        rig = gameObject.GetComponent<Rigidbody>();
        cam = GameObject.Find("CameraView");

        origSpeed = stat.speed;
        origRotSpeed = stat.rotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        carMove();

        // 공중에서 차량의 각도를 제한하고 싶다.
        float rx = transform.eulerAngles.z;
        float rz = transform.eulerAngles.x;
        if (rx > 60 && rx < 180)
            rx = 60;
        else if (rx < 300 && rx > 180)
            rx = 300;
        if (rz > 60 && rz < 180)
            rz = 60;
        else if (rz < 300 && rz > 180)
            rz = 300;
        transform.eulerAngles = new Vector3(rz, transform.eulerAngles.y, rx);

        // 현재 직진인지 좌회전인지 우회전인지 확인
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

    // 아래로 Raycast를 쏴서 차량이 공중에 있는지 확인하고 싶다.
    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f + Vector3.forward, -Vector3.up, 0.8f, LayerMask.GetMask("Block"))
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
                transform.eulerAngles += InputManager.Instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            // 속도가 마이너스라면(후진 중이라면)
            else
                // 버튼을 누르는 반대방향으로 차량이 회전한다.    
                transform.eulerAngles -= InputManager.Instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
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
            stat.speed = origSpeed;
            stat.rotSpeed = origRotSpeed;
            isDrift = false;
            // 공중에선 차량이 바라보는 방향 중 Y축 방향을 배제하고 이동하고 싶다.
            dir.y = 0;
            // 공중에서 드리프트를 누르면 차량 좌우 방향을 전환하고 싶다.
            if (InputManager.Instance.Drift)
            {
                transform.eulerAngles += InputManager.Instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // 가속도 운동을 구현하고 싶다.
        if (InputManager.Instance.Accel)
        {
            lerp = Mathf.Lerp(lerp, stat.speed, Time.deltaTime / 10 * stat.accelPower);
            transform.position += lerp * dir * Time.deltaTime;
            // 직진 시에 엔진 사운드를 재생한다.
            SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
        }
        else if (InputManager.Instance.Brake)
        {
            lerp = Mathf.Lerp(lerp, -stat.speed / 3, Time.deltaTime / 10 * stat.brakePower);
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
        rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // 지상에서 쉬프트키를 누르면 드리프트
        if(IsGrounded())
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
                stat.speed = origSpeed;
                stat.rotSpeed = origRotSpeed;
                // 드리프트 소리 멈춤
                SoundManager.Instance.Stop(SoundManager.Sound.Drift);
                isDrift = false;
            }
        }

        // 컨트롤 키를 누르면 부스터
        if (InputManager.Instance.Boost)
        {
            skill.Boost();
            // 부스터 소리 재생
            SoundManager.Instance.Play(boostClip, SoundManager.Sound.Booster);
            isBooster = true;
        }
        else if (InputManager.Instance.BoostEnd || stat.boostGauge < 0)
        {
            // 부스터 키를 떼거나 부스트 게이지가 없으면 속도를 원상복귀
            stat.speed = origSpeed;
            stat.rotSpeed = origRotSpeed;
            // 부스터 소리 멈춤
            SoundManager.Instance.Stop(SoundManager.Sound.Booster);
            isBooster = false;
        }

        // 부스터와 드리프트를 동시에 할 때의 경우를 따로 빼서 처리
        if (isDrift && isBooster)
        {
            stat.speed = origSpeed;
        }

        // 스페이스바를 누를 때 지상에 있으면 점프
        if (InputManager.Instance.Jump)
        {
            if (IsGrounded()) 
                skill.Jump();
            // 점프 소리 재생
            SoundManager.Instance.Play(jumpClip, SoundManager.Sound.Jump);
        }
    }

    // 충돌했을 때 장애물을 뚫는 오류를 해결하기 위해 작성
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            // 충돌 시 차량의 속도를 0으로 만든다.
            lerp = 0;
            SoundManager.Instance.Play(collideClip, SoundManager.Sound.Collide);
        }
    }
}
