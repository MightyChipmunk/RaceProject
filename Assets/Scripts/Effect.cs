using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    float lerp = 0;

    PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        changeWheelAngle();
    }

    public void changeWheelAngle()
    {
        // ���� ���¿� ���� �� ������ �ٲٰ� �ʹ�.
        // ������ �������� �����ְų� ���������� �帮��Ʈ ���̶�� �� ������ �������� ������.
        if (((pc._state == PlayerController.PlayerState.Left) && (!pc.isDrift) || ((pc._state == PlayerController.PlayerState.Right) && (pc.isDrift))))
        {
            lerp = Mathf.Lerp(lerp, -30, Time.deltaTime * 4);
            transform.GetChild(1).localEulerAngles = new Vector3(0, lerp, 0);
            transform.GetChild(3).localEulerAngles = new Vector3(0, lerp, 0);
        }
        // ������ ���������� �����ְų� �������� �帮��Ʈ ���̶�� �� ������ ���������� ������.
        else if (((pc._state == PlayerController.PlayerState.Right) && (!pc.isDrift) || ((pc._state == PlayerController.PlayerState.Left) && (pc.isDrift))))
        {
            lerp = Mathf.Lerp(lerp, 30, Time.deltaTime * 4);
            transform.GetChild(1).localEulerAngles = new Vector3(0, lerp, 0);
            transform.GetChild(3).localEulerAngles = new Vector3(0, lerp, 0);
        }
        // ������ �������̶�� ���� ������ ������.
        else if (pc._state == PlayerController.PlayerState.Front)
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * 4);
            transform.GetChild(1).localEulerAngles = new Vector3(0, lerp, 0);
            transform.GetChild(3).localEulerAngles = new Vector3(0, lerp, 0);
        }
    }

    public void skidMark() { }
    public void boostFire() { }
    public void playSound() { }
}
