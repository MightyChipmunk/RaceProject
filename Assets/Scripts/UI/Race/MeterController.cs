using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    Transform GaugeBarTranform;
    const float MAX_SPEED_ANGLE = -90;
    const float ZERO_SPEED_ANGLE = 180;
    float speedMax;
    float speed;

    float lerp = 0;

    GameObject player;
    PlayerController pc;
    PlayerStat stat;

    private void Start()
    {
        GaugeBarTranform = transform.Find("GaugeBar");
        speed = 0f;

        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
        stat = player.GetComponent<PlayerStat>();

        speedMax = stat.Speed;
    }


    private void Update()
    {
        speed = pc.Lerp;
        GaugeBarTranform.eulerAngles = new Vector3(0, 0, GetspeedRotation());
    }

    private float GetspeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        speedNormalized = Mathf.Clamp(speedNormalized, -1, 1);

        if (speed >= 0)
            lerp = Mathf.Lerp(lerp, speedNormalized, Time.deltaTime);
        else
            lerp = Mathf.Lerp(lerp, -speedNormalized, Time.deltaTime);

        return ZERO_SPEED_ANGLE - lerp * totalAngleSize;
    }
}
