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

        // ���󿡼� A, D Ű�� ���� �� ���� ������ �����ϰ�ʹ�.
        if (IsGrounded())
        {
            // ���� �ӵ��� 0 �̻��̸�
            if (lerp >= 0)
                // ��ư�� ������ �������� ������ ȸ���Ѵ�.
                transform.eulerAngles += x * stat.rotSpeed * Time.deltaTime * Vector3.up;
            // �ӵ��� ���̳ʽ����(���� ���̶��)
            else
                // ��ư�� ������ �ݴ�������� ������ ȸ���Ѵ�.    
                transform.eulerAngles -= x * stat.rotSpeed * Time.deltaTime * Vector3.up;
        }
        // ���߿��� A, D Ű�� ���� �� ������ �¿�� �̵��ϰ� �ʹ�.
        else
        {
            // ���� ������ �ƴ� ī�޶� �ٶ󺸴� ������ �������� �¿�� �̵��ϰ� �ʹ�.
            Vector3 camDir = camera.transform.rotation * Vector3.right;
            transform.position += x * 10 * Time.deltaTime * camDir;
        }

        // ���߿��� ������ ������ �����ϰ� �ʹ�.
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

        // ������ �ٶ󺸴� �������� ��/���� �ϰ�ʹ�.
        Vector3 dir = transform.rotation * Vector3.forward;

        // ���߿��� ������ �ٶ󺸴� ���� �� Y�� ������ �����ϰ� �̵��ϰ� �ʹ�.
        // ���߿��� Shift+A, D�� ������ ���� �¿� ������ ��ȯ�ϰ� �ʹ�.
        if (!IsGrounded())
        {
            dir.y = 0;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.eulerAngles += x * stat.rotSpeed * Time.deltaTime * Vector3.up;
            }
        }

        dir.Normalize();

        // ���ӵ� ��� �����ϰ� �ʹ�.
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

        // ü���ð��� �ø��� �ʹ�.
        rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);
        Debug.Log(IsGrounded());
    }

    // �Ʒ��� Raycast�� ���� ������ ���߿� �ִ��� Ȯ���ϰ� �ʹ�.
    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f + Vector3.forward, -Vector3.up, 1.0f, LayerMask.GetMask("Block"))
            || Physics.Raycast(transform.position + Vector3.up * 0.5f - Vector3.forward, -Vector3.up, 1.0f, LayerMask.GetMask("Block"));
    }
}
