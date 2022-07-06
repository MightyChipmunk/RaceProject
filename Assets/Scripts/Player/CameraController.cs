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
        // ī�޶�� �÷��̾� ���� ���͸� ���Ѵ�.
        Vector3 dir = player.transform.position - transform.GetChild(0).position;
        dir.Normalize();
        // ī�޶� ����� ��ġ�� �׻� �÷��̾��� ��ġ�̴�.
        transform.position = player.transform.position;
        // ī�޶� ����� ���Ϸ� ������ 360�� �ʰ��ϰų� 0 �̸��� �� ������ ������ �߻��Ѵ�.
        // ���� ������ ����� 360�� �ʰ��� ���� 0 �̸��� �ɶ��� ���� ����Ѵ�.
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
        // �� �� ��Ȳ������ �÷��̾ �ٶ󺸴� ������ õõ�� �ٶ󺸵��� �Ѵ�.
        else
        {
            transform.eulerAngles += (player.transform.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime * rotSpeed * Vector3.up;
        }

        // �ν��͸� �ϴ� ��Ȳ���� ī�޶� �ڷ� �а�ʹ�.
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