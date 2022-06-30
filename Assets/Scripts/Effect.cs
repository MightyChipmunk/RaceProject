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
        // 차량 상태에 따라 휠 각도를 바꾸고 싶다.
        // 차량이 왼쪽으로 돌고있거나 오른쪽으로 드리프트 중이라면 휠 각도를 왼쪽으로 돌린다.
        if (((pc._state == PlayerController.PlayerState.Left) && (!pc.isDrift) || ((pc._state == PlayerController.PlayerState.Right) && (pc.isDrift))))
        {
            lerp = Mathf.Lerp(lerp, -30, Time.deltaTime * 4);
            transform.GetChild(1).localEulerAngles = new Vector3(0, lerp, 0);
            transform.GetChild(3).localEulerAngles = new Vector3(0, lerp, 0);
        }
        // 차량이 오른쪽으로 돌고있거나 왼쪽으로 드리프트 중이라면 휠 각도를 오른쪽으로 돌린다.
        else if (((pc._state == PlayerController.PlayerState.Right) && (!pc.isDrift) || ((pc._state == PlayerController.PlayerState.Left) && (pc.isDrift))))
        {
            lerp = Mathf.Lerp(lerp, 30, Time.deltaTime * 4);
            transform.GetChild(1).localEulerAngles = new Vector3(0, lerp, 0);
            transform.GetChild(3).localEulerAngles = new Vector3(0, lerp, 0);
        }
        // 차량이 직진중이라면 휠을 앞으로 돌린다.
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
