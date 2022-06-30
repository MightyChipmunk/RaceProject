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
    GameObject camera;

    public PlayerState _state = PlayerState.Front;

    // Start is called before the first frame update
    void Start()
    {
        stat = gameObject.GetComponent<PlayerStat>();
        skill = gameObject.GetComponent<PlayerSkill>();
        rig = gameObject.GetComponent<Rigidbody>();
        camera = GameObject.Find("CameraView");

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
        if (InputManager.instance.Horizon > 0)
        {
            _state = PlayerState.Right;
        }
        else if (InputManager.instance.Horizon < 0)
        {
            _state = PlayerState.Left;
        }
        else if (InputManager.instance.Horizon == 0)
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
                transform.eulerAngles += InputManager.instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            // 속도가 마이너스라면(후진 중이라면)
            else
                // 버튼을 누르는 반대방향으로 차량이 회전한다.    
                transform.eulerAngles -= InputManager.instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
        }
        // 공중에서 A, D 키를 누를 때 차량을 좌우로 이동하고 싶다.
        else
        {
            // 차량 기준이 아닌 카메라가 바라보는 방향을 기준으로 좌우로 이동하고 싶다.
            Vector3 camDir = camera.transform.rotation * Vector3.right;
            transform.position += InputManager.instance.Horizon * 10 * Time.deltaTime * camDir;
        }

        // 차량이 바라보는 방향으로 전/후진 하고싶다.
        Vector3 dir = transform.rotation * Vector3.forward;
        // 공중에선 차량이 바라보는 방향 중 Y축 방향을 배제하고 이동하고 싶다.
        // 공중에서 드리프트를 누르면 차량 좌우 방향을 전환하고 싶다.
        if (!IsGrounded())
        {
            // 만약 드리프트 중간에 공중에 뜨면 드리프트 중단
            stat.speed = origSpeed;
            stat.rotSpeed = origRotSpeed;
            isDrift = false;
            dir.y = 0;
            if (InputManager.instance.Drift)
            {
                transform.eulerAngles += InputManager.instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // 가속도 운동을 구현하고 싶다.
        if (InputManager.instance.Accel)
        {
            lerp = Mathf.Lerp(lerp, stat.speed, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }
        else if (InputManager.instance.Brake)
        {
            lerp = Mathf.Lerp(lerp, -stat.speed / 3, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }

        // 체공시간을 늘리고 싶다.
        rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // 지상에서 쉬프트키를 누르면 드리프트
        if(IsGrounded())
        {
            if (InputManager.instance.Drift)
            {
                Debug.Log("drift");
                skill.Drift();
                isDrift = true;
            }
            else if (InputManager.instance.DriftEnd)
            {
                stat.speed = origSpeed;
                stat.rotSpeed = origRotSpeed;
                isDrift = false;
            }
        }

        // 컨트롤 키를 누르면 부스터
        if (InputManager.instance.Boost)
        {
            skill.Boost();
            isBooster = true;
        }
        else if (InputManager.instance.BoostEnd)
        {
            stat.speed = origSpeed;
            stat.rotSpeed = origRotSpeed;
            isBooster = false;
        }

        // 부스터와 드리프트를 동시에 할 때의 경우를 따로 빼서 처리
        if (isDrift && isBooster)
        {
            stat.speed = origSpeed;
        }

        // 스페이스바를 누를 때 지상에 있으면 점프
        if (InputManager.instance.Jump)
        {
            if (IsGrounded()) 
                skill.Jump();
        }
    }
}
