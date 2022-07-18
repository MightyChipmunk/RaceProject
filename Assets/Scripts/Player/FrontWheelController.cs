using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheelController : MonoBehaviour
{
    float lerp = 0;
    float rotX = 0;

    PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        changeWheelAngle();
    }

    public void changeWheelAngle()
    {
        rotX += pc.Lerp;
        // ���� ���¿� ���� �� ������ �ٲٰ� �ʹ�.
        // ������ �������� �����ְų� ���������� �帮��Ʈ ���̶�� �� ������ �������� ������.
        if (((pc._state == PlayerController.PlayerState.Left) && (!pc.IsDrift) || ((pc._state == PlayerController.PlayerState.Right) && (pc.IsDrift))))
        {
            lerp = Mathf.Lerp(lerp, -30, Time.deltaTime * 4);
            transform.localEulerAngles = new Vector3(rotX, lerp, 0);
        }
        // ������ ���������� �����ְų� �������� �帮��Ʈ ���̶�� �� ������ ���������� ������.
        else if (((pc._state == PlayerController.PlayerState.Right) && (!pc.IsDrift) || ((pc._state == PlayerController.PlayerState.Left) && (pc.IsDrift))))
        {
            lerp = Mathf.Lerp(lerp, 30, Time.deltaTime * 4);
            transform.localEulerAngles = new Vector3(rotX, lerp, 0);
        }
        // ������ �������̶�� ���� ������ ������.
        else if (pc._state == PlayerController.PlayerState.Front)
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * 4);
            transform.localEulerAngles = new Vector3(rotX, lerp, 0);
        }
    }
}
