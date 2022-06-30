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

        // ���� �������� ��ȸ������ ��ȸ������ Ȯ��
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

    // �Ʒ��� Raycast�� ���� ������ ���߿� �ִ��� Ȯ���ϰ� �ʹ�.
    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f + Vector3.forward, -Vector3.up, 0.8f, LayerMask.GetMask("Block"))
            || Physics.Raycast(transform.position + Vector3.up * 0.5f - Vector3.forward, -Vector3.up, 0.8f, LayerMask.GetMask("Block"));
    }

    public void carMove()
    {

        // ���󿡼� A, D Ű�� ���� �� ���� ������ �����ϰ�ʹ�.
        if (IsGrounded())
        {
            // ���� �ӵ��� 0 �̻��̸�
            if (lerp >= -0.1f)
                // ��ư�� ������ �������� ������ ȸ���Ѵ�.
                transform.eulerAngles += InputManager.instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            // �ӵ��� ���̳ʽ����(���� ���̶��)
            else
                // ��ư�� ������ �ݴ�������� ������ ȸ���Ѵ�.    
                transform.eulerAngles -= InputManager.instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
        }
        // ���߿��� A, D Ű�� ���� �� ������ �¿�� �̵��ϰ� �ʹ�.
        else
        {
            // ���� ������ �ƴ� ī�޶� �ٶ󺸴� ������ �������� �¿�� �̵��ϰ� �ʹ�.
            Vector3 camDir = camera.transform.rotation * Vector3.right;
            transform.position += InputManager.instance.Horizon * 10 * Time.deltaTime * camDir;
        }

        // ������ �ٶ󺸴� �������� ��/���� �ϰ�ʹ�.
        Vector3 dir = transform.rotation * Vector3.forward;
        // ���߿��� ������ �ٶ󺸴� ���� �� Y�� ������ �����ϰ� �̵��ϰ� �ʹ�.
        // ���߿��� �帮��Ʈ�� ������ ���� �¿� ������ ��ȯ�ϰ� �ʹ�.
        if (!IsGrounded())
        {
            // ���� �帮��Ʈ �߰��� ���߿� �߸� �帮��Ʈ �ߴ�
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

        // ���ӵ� ��� �����ϰ� �ʹ�.
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

        // ü���ð��� �ø��� �ʹ�.
        rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // ���󿡼� ����ƮŰ�� ������ �帮��Ʈ
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

        // ��Ʈ�� Ű�� ������ �ν���
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

        // �ν��Ϳ� �帮��Ʈ�� ���ÿ� �� ���� ��츦 ���� ���� ó��
        if (isDrift && isBooster)
        {
            stat.speed = origSpeed;
        }

        // �����̽��ٸ� ���� �� ���� ������ ����
        if (InputManager.instance.Jump)
        {
            if (IsGrounded()) 
                skill.Jump();
        }
    }
}
