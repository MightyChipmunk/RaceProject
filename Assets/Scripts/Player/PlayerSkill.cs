using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    PlayerStat stat;
    public float origSpeed;
    public float origRotSpeed;
    Rigidbody rig;
    public float jumpForce = 300;
    PlayerController pc;


    // Start is called before the first frame update
    void Start()
    {
        stat = gameObject.GetComponent<PlayerStat>();

        origRotSpeed = stat.rotSpeed;
        origSpeed = stat.speed;

        rig = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drift()
    {
        Vector3 dirDrift = transform.rotation * Vector3.left;

        stat.rotSpeed = origRotSpeed * 2;
        stat.speed = origSpeed / 2;

        transform.position += InputManager.Instance.Horizon * dirDrift * 3 * Time.deltaTime;

        if(stat.boostGauge < 10)
        {
            stat.boostGauge += 2.0f * Time.deltaTime;
        }
    }

    public void Boost()
    {
        
        if (stat.boostGauge >= 0)
        {
            stat.speed = origSpeed * 2;
            stat.boostGauge -= 4.0f * Time.deltaTime;
        }
    }

    public void Jump()
    {
        rig.AddForce(Vector3.up * jumpForce);
    }

}
