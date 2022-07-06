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
                transform.eulerAngles += InputManager.Instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            // �ӵ��� ���̳ʽ����(���� ���̶��)
            else
                // ��ư�� ������ �ݴ�������� ������ ȸ���Ѵ�.    
                transform.eulerAngles -= InputManager.Instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
        }
        // ���߿��� A, D Ű�� ���� �� ������ �¿�� �����̵��ϰ� �ʹ�.
        else
        {
            // ���� ������ �ƴ� ī�޶� �ٶ󺸴� ������ �������� �¿�� �̵��ϰ� �ʹ�.
            Vector3 camDir = cam.transform.rotation * Vector3.right;
            transform.position += InputManager.Instance.Horizon * 10 * Time.deltaTime * camDir;
        }

        // ������ �ٶ󺸴� �������� ��/���� �ϰ�ʹ�.
        Vector3 dir = transform.rotation * Vector3.forward;
        if (!IsGrounded())
        {
            // ���� �帮��Ʈ �߰��� ���߿� �߸� �帮��Ʈ �ߴ�
            stat.speed = origSpeed;
            stat.rotSpeed = origRotSpeed;
            isDrift = false;
            // ���߿��� ������ �ٶ󺸴� ���� �� Y�� ������ �����ϰ� �̵��ϰ� �ʹ�.
            dir.y = 0;
            // ���߿��� �帮��Ʈ�� ������ ���� �¿� ������ ��ȯ�ϰ� �ʹ�.
            if (InputManager.Instance.Drift)
            {
                transform.eulerAngles += InputManager.Instance.Horizon * stat.rotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // ���ӵ� ��� �����ϰ� �ʹ�.
        if (InputManager.Instance.Accel)
        {
            lerp = Mathf.Lerp(lerp, stat.speed, Time.deltaTime / 10 * stat.accelPower);
            transform.position += lerp * dir * Time.deltaTime;
            // ���� �ÿ� ���� ���带 ����Ѵ�.
            SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
        }
        else if (InputManager.Instance.Brake)
        {
            lerp = Mathf.Lerp(lerp, -stat.speed / 3, Time.deltaTime / 10 * stat.brakePower);
            transform.position += lerp * dir * Time.deltaTime;
            // �극��ũ�� ������ ���� ���带 ������ �����.
            if (lerp > 0)
                SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
            // ���� �ӵ��� 0 �Ʒ���� ��ġ ������ -(�ӵ�) ���� �Ѱ��ش�.
            else
                SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, -lerp / origSpeed * 2);
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime / 5);
            transform.position += lerp * dir * Time.deltaTime;
            // ������ �� ������ ���� ���带 ������ �����.
            SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
        }

        // ü���ð��� �ø��� �ʹ�.
        rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // ���󿡼� ����ƮŰ�� ������ �帮��Ʈ
        if(IsGrounded())
        {
            if (InputManager.Instance.Drift)
            {
                skill.Drift();
                // �帮��Ʈ �Ҹ� ���
                SoundManager.Instance.Play(driftClip, SoundManager.Sound.Drift);
                isDrift = true;
            }
            else if (InputManager.Instance.DriftEnd)
            {
                // �帮��Ʈ Ű�� ���� �ӵ��� ���󺹱�
                stat.speed = origSpeed;
                stat.rotSpeed = origRotSpeed;
                // �帮��Ʈ �Ҹ� ����
                SoundManager.Instance.Stop(SoundManager.Sound.Drift);
                isDrift = false;
            }
        }

        // ��Ʈ�� Ű�� ������ �ν���
        if (InputManager.Instance.Boost)
        {
            skill.Boost();
            // �ν��� �Ҹ� ���
            SoundManager.Instance.Play(boostClip, SoundManager.Sound.Booster);
            isBooster = true;
        }
        else if (InputManager.Instance.BoostEnd || stat.boostGauge < 0)
        {
            // �ν��� Ű�� ���ų� �ν�Ʈ �������� ������ �ӵ��� ���󺹱�
            stat.speed = origSpeed;
            stat.rotSpeed = origRotSpeed;
            // �ν��� �Ҹ� ����
            SoundManager.Instance.Stop(SoundManager.Sound.Booster);
            isBooster = false;
        }

        // �ν��Ϳ� �帮��Ʈ�� ���ÿ� �� ���� ��츦 ���� ���� ó��
        if (isDrift && isBooster)
        {
            stat.speed = origSpeed;
        }

        // �����̽��ٸ� ���� �� ���� ������ ����
        if (InputManager.Instance.Jump)
        {
            if (IsGrounded()) 
                skill.Jump();
            // ���� �Ҹ� ���
            SoundManager.Instance.Play(jumpClip, SoundManager.Sound.Jump);
        }
    }

    // �浹���� �� ��ֹ��� �մ� ������ �ذ��ϱ� ���� �ۼ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            // �浹 �� ������ �ӵ��� 0���� �����.
            lerp = 0;
            SoundManager.Instance.Play(collideClip, SoundManager.Sound.Collide);
        }
    }
}
