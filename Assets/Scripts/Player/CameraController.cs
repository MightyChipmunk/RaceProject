using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    PlayerController pc;

    public float rotSpeed = 4;
    float lerp = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라와 플레이어 간의 벡터를 구한다.
        Vector3 dir = player.transform.position - transform.GetChild(0).position;
        dir.Normalize();
        // 카메라 뷰어의 위치는 항상 플레이어의 위치이다.
        transform.position = player.transform.position;
        // 카메라 뷰어의 오일러 각도가 360을 초과하거나 0 미만이 될 때마다 오류가 발생한다.
        // 따라서 절댓값을 계산해 360을 초과할 때와 0 미만이 될때를 따로 계산한다.
        if (Math.Abs(player.transform.eulerAngles.y - transform.eulerAngles.y) > 180)
        {
            if (player.transform.eulerAngles.y - transform.eulerAngles.y >= 0)
            {
                transform.eulerAngles += (player.transform.eulerAngles.y - transform.eulerAngles.y - 360) * Time.deltaTime * rotSpeed * Vector3.up;
            }
            else if (player.transform.eulerAngles.y - transform.eulerAngles.y < 0)
            {
                transform.eulerAngles += (player.transform.eulerAngles.y - transform.eulerAngles.y + 360) * Time.deltaTime * rotSpeed * Vector3.up;
            }
        }
        // 그 외 상황에서는 플레이어가 바라보는 방향을 천천히 바라보도록 한다.
        else
        {
            transform.eulerAngles += (player.transform.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime * rotSpeed * Vector3.up;
        }

        // 부스터를 하는 상황에는 카메라를 뒤로 밀고싶다.
        if(pc.isBooster)
        {
            lerp = Mathf.Lerp(lerp, 1.5f, Time.deltaTime);
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime);
        }
        transform.position = player.transform.position - dir * lerp;
    }
}