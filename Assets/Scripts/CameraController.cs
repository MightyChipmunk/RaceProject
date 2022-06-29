using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    public float rotSpeed = 4;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
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
        else
        {
            transform.eulerAngles += (player.transform.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime * rotSpeed * Vector3.up;
        }
    }
}