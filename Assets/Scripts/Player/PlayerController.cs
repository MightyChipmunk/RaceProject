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

        
        // ���߿��� ������ ������ �����ϰ� �ʹ�.
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

        // ���� �������� ��ȸ������ ��ȸ������ Ȯ��
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

    // �Ʒ��� Raycast�� ���� ������ ���߿� �ִ��� Ȯ���ϰ� �ʹ�.
    bool IsGrounded() 
    {
        return  Physics.Raycast(transform.position + Vector3.up * 0.5f - Vector3.forward, -Vector3.up, 0.8f, LayerMask.GetMask("Block"))
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
                transform.eulerAngles += InputManager.Instance.Horizon * stat.RotSpeed * Time.deltaTime * Vector3.up;
            // �ӵ��� ���̳ʽ����(���� ���̶��)
            else
                // ��ư�� ������ �ݴ�������� ������ ȸ���Ѵ�.    
                transform.eulerAngles -= InputManager.Instance.Horizon * stat.RotSpeed * Time.deltaTime * Vector3.up;
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
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            isDrift = false;
            // ���߿��� ������ �ٶ󺸴� ���� �� Y�� ������ �����ϰ� �̵��ϰ� �ʹ�.
            dir.y = 0;
            // ���߿��� �帮��Ʈ�� ������ ���� �¿� ������ ��ȯ�ϰ� �ʹ�.
            if (InputManager.Instance.Drift)
            {
                transform.eulerAngles += InputManager.Instance.Horizon * stat.RotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // ���ӵ� ��� �����ϰ� �ʹ�.
        if (InputManager.Instance.Accel)
        {
            lerp = Mathf.Lerp(lerp, stat.Speed, Time.deltaTime / 10 * stat.AccelPower);
            transform.position += lerp * dir * Time.deltaTime;
            // ���� �ÿ� ���� ���带 ����Ѵ�.
            SoundManager.Instance.Play(engineClip, SoundManager.Sound.Engine, lerp / origSpeed * 2);
        }
        else if (InputManager.Instance.Brake)
        {
            lerp = Mathf.Lerp(lerp, -stat.Speed / 3, Time.deltaTime / 10 * stat.BrakePower);
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
        //rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // �����̽��ٸ� ���� �� ���� ������ ����
        if (InputManager.Instance.Jump)
        {
            if (IsGrounded())
                skill.Jump();
            // ���� �Ҹ� ���
            SoundManager.Instance.Stop(SoundManager.Sound.Drift);
            SoundManager.Instance.Play(jumpClip, SoundManager.Sound.Jump);
        }

        if (!IsGrounded())
            SoundManager.Instance.Stop(SoundManager.Sound.Drift); ;

        // ���󿡼� ����ƮŰ�� ������ �帮��Ʈ
        if (IsGrounded())
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
                stat.Speed = origSpeed;
                stat.RotSpeed = origRotSpeed;
                // �帮��Ʈ �Ҹ� ����
                SoundManager.Instance.Stop(SoundManager.Sound.Drift);
                isDrift = false;
            }
        }

        // ��Ʈ�� Ű�� ������ �ν���
        if (InputManager.Instance.Boost && stat.BoostGauge >= 0)
        {
            skill.Boost();
            // �ν��� �Ҹ� ���
            SoundManager.Instance.Play(boostClip, SoundManager.Sound.Booster);
            isBooster = true;
        }
        else if (InputManager.Instance.BoostEnd || stat.BoostGauge < 0)
        {
            // �ν��� Ű�� ���ų� �ν�Ʈ �������� ������ �ӵ��� ���󺹱�
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            // �ν��� �Ҹ� ����
            SoundManager.Instance.Stop(SoundManager.Sound.Booster);
            isBooster = false;
        }

        // �ν��Ϳ� �帮��Ʈ�� ���ÿ� �� ���� ��츦 ���� ���� ó��
        if (isDrift && isBooster)
        {
            stat.Speed = origSpeed;
        }

        
    }

    public void carMove_Mul()
    {

        // ���󿡼� A, D Ű�� ���� �� ���� ������ �����ϰ�ʹ�.
        if (IsGrounded())
        {
            // ���� �ӵ��� 0 �̻��̸�
            if (lerp >= -0.1f)
                // ��ư�� ������ �������� ������ ȸ���Ѵ�.
                transform.eulerAngles += InputManager.Instance.Horizon2 * stat.RotSpeed * Time.deltaTime * Vector3.up;
            // �ӵ��� ���̳ʽ����(���� ���̶��)
            else
                // ��ư�� ������ �ݴ�������� ������ ȸ���Ѵ�.    
                transform.eulerAngles -= InputManager.Instance.Horizon2 * stat.RotSpeed * Time.deltaTime * Vector3.up;
        }
        // ���߿��� A, D Ű�� ���� �� ������ �¿�� �����̵��ϰ� �ʹ�.
        else
        {
            // ���� ������ �ƴ� ī�޶� �ٶ󺸴� ������ �������� �¿�� �̵��ϰ� �ʹ�.
            Vector3 camDir = cam.transform.rotation * Vector3.right;
            transform.position += InputManager.Instance.Horizon2 * 10 * Time.deltaTime * camDir;
        }

        // ������ �ٶ󺸴� �������� ��/���� �ϰ�ʹ�.
        Vector3 dir = transform.rotation * Vector3.forward;
        if (!IsGrounded())
        {
            // ���� �帮��Ʈ �߰��� ���߿� �߸� �帮��Ʈ �ߴ�
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            isDrift = false;
            // ���߿��� ������ �ٶ󺸴� ���� �� Y�� ������ �����ϰ� �̵��ϰ� �ʹ�.
            dir.y = 0;
            // ���߿��� �帮��Ʈ�� ������ ���� �¿� ������ ��ȯ�ϰ� �ʹ�.
            if (InputManager.Instance.Drift2)
            {
                transform.eulerAngles += InputManager.Instance.Horizon2 * stat.RotSpeed * Time.deltaTime * Vector3.up;
            }
        }
        dir.Normalize();

        // ���ӵ� ��� �����ϰ� �ʹ�.
        if (InputManager.Instance.Accel2)
        {
            lerp = Mathf.Lerp(lerp, stat.Speed, Time.deltaTime / 10 * stat.AccelPower);
            transform.position += lerp * dir * Time.deltaTime;
            // ���� �ÿ� ���� ���带 ����Ѵ�.
            SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, lerp / origSpeed * 2);
        }
        else if (InputManager.Instance.Brake2)
        {
            lerp = Mathf.Lerp(lerp, -stat.Speed / 3, Time.deltaTime / 10 * stat.BrakePower);
            transform.position += lerp * dir * Time.deltaTime;
            // �극��ũ�� ������ ���� ���带 ������ �����.
            if (lerp > 0)
                SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, lerp / origSpeed * 2);
            // ���� �ӵ��� 0 �Ʒ���� ��ġ ������ -(�ӵ�) ���� �Ѱ��ش�.
            else
                SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, -lerp / origSpeed * 2);
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime / 5);
            transform.position += lerp * dir * Time.deltaTime;
            // ������ �� ������ ���� ���带 ������ �����.
            SoundManager_Multi.Instance.Play(engineClip, SoundManager_Multi.Sound.Engine, lerp / origSpeed * 2);
        }

        // ü���ð��� �ø��� �ʹ�.
        //rig.AddForce(Vector3.up * 10.0f * Time.deltaTime);

        // �����̽��ٸ� ���� �� ���� ������ ����
        if (InputManager.Instance.Jump2)
        {
            if (IsGrounded())
                skill.Jump();
            // ���� �Ҹ� ���
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift);
            SoundManager_Multi.Instance.Play(jumpClip, SoundManager_Multi.Sound.Jump);
        }

        if (!IsGrounded())
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift); ;

        // ���󿡼� ����ƮŰ�� ������ �帮��Ʈ
        if (IsGrounded())
        {
            if (InputManager.Instance.Drift2)
            {
                skill.Drift();
                // �帮��Ʈ �Ҹ� ���
                SoundManager_Multi.Instance.Play(driftClip, SoundManager_Multi.Sound.Drift);
                isDrift = true;
            }
            else if (InputManager.Instance.DriftEnd2)
            {
                // �帮��Ʈ Ű�� ���� �ӵ��� ���󺹱�
                stat.Speed = origSpeed;
                stat.RotSpeed = origRotSpeed;
                // �帮��Ʈ �Ҹ� ����
                SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift);
                isDrift = false;
            }
        }

        // ��Ʈ�� Ű�� ������ �ν���
        if (InputManager.Instance.Boost2 && stat.BoostGauge >= 0)
        {
            skill.Boost();
            // �ν��� �Ҹ� ���
            SoundManager_Multi.Instance.Play(boostClip, SoundManager_Multi.Sound.Booster);
            isBooster = true;
        }
        else if (InputManager.Instance.BoostEnd2 || stat.BoostGauge < 0)
        {
            // �ν��� Ű�� ���ų� �ν�Ʈ �������� ������ �ӵ��� ���󺹱�
            stat.Speed = origSpeed;
            stat.RotSpeed = origRotSpeed;
            // �ν��� �Ҹ� ����
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Booster);
            isBooster = false;
        }

        // �ν��Ϳ� �帮��Ʈ�� ���ÿ� �� ���� ��츦 ���� ���� ó��
        if (isDrift && isBooster)
        {
            stat.Speed = origSpeed;
        }
    }

    // �浹���� �� ��ֹ��� �մ� ������ �ذ��ϱ� ���� �ۼ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && !is2P)
        {
            // �浹 �� ������ �ӵ��� 0���� �����.
            lerp = 0;
            SoundManager.Instance.Play(collideClip, SoundManager.Sound.Collide);
        }
        else if (collision.gameObject.layer == 9 && is2P)
        {
            // �浹 �� ������ �ӵ��� 0���� �����.
            lerp = 0;
            SoundManager_Multi.Instance.Play(collideClip, SoundManager_Multi.Sound.Collide);
        }

        if (collision.gameObject.name.Contains("Player"))
        {
            SoundManager.Instance.Play(collideClip, SoundManager.Sound.Collide);
        }
    }
}
