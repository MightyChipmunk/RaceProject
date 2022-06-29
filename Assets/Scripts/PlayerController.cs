using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float lerp = 0;
    PlayerStat stat;
    PlayerSkill skill;
    Rigidbody rig;
    GameObject camera;
    Collider collider;

    float distToGround;

    // Start is called before the first frame update
    void Start()
    {
        stat = gameObject.GetComponent<PlayerStat>();
        skill = gameObject.GetComponent<PlayerSkill>();
        rig = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<BoxCollider>();
        camera = GameObject.Find("CameraView");

        distToGround = collider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        // 지상에서 A, D 키를 누를 때 차량 각도를 조정하고싶다.
        if (IsGrounded())
        {
            // 만약 속도가 0 이상이면
            if (lerp >= 0)
                // 버튼을 누르는 방향으로 차량이 회전한다.
                transform.eulerAngles += x * stat.rotSpeed * Time.deltaTime * Vector3.up;
            // 속도가 마이너스라면(후진 중이라면)
            else
                // 버튼을 누르는 반대방향으로 차량이 회전한다.    
                transform.eulerAngles -= x * stat.rotSpeed * Time.deltaTime * Vector3.up;
        }
        // 공중에서 A, D 키를 누를 때 차량을 좌우로 이동하고 싶다.
        else
        {
            // 차량 기준이 아닌 카메라가 바라보는 방향을 기준으로 좌우로 이동하고 싶다.
            Vector3 camDir = camera.transform.rotation * Vector3.right;
            transform.position += x * 10 * Time.deltaTime * camDir;
        }

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

        // 차량이 바라보는 방향으로 전/후진 하고싶다.
        Vector3 dir = transform.rotation * Vector3.forward;

        // 공중에선 차량이 바라보는 방향 중 Y축 방향을 배제하고 이동하고 싶다.
        // 공중에서 Shift+A, D를 누르면 차량 좌우 방향을 전환하고 싶다.
        if (!IsGrounded())
        {
            dir.y = 0;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.eulerAngles += x * stat.rotSpeed * Time.deltaTime * Vector3.up;
            }
        }

        dir.Normalize();

        // 가속도 운동을 구현하고 싶다.
        if (Input.GetKey(KeyCode.W))
        {
            lerp = Mathf.Lerp(lerp, stat.speed, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
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
        Debug.Log(IsGrounded());
    }

    // 아래로 Raycast를 쏴서 차량이 공중에 있는지 확인하고 싶다.
    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f + Vector3.forward, -Vector3.up, 1.0f, LayerMask.GetMask("Block"))
            || Physics.Raycast(transform.position + Vector3.up * 0.5f - Vector3.forward, -Vector3.up, 1.0f, LayerMask.GetMask("Block"));
    }
}
