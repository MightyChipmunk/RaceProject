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

        origRotSpeed = stat.RotSpeed;
        origSpeed = stat.Speed;

        rig = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drift()
    {
        Vector3 dirDrift = transform.rotation * Vector3.left;

        stat.RotSpeed = origRotSpeed * 2;
        stat.Speed = origSpeed / 2;

        transform.position += InputManager.Instance.Horizon * dirDrift * 3 * Time.deltaTime;

        if(stat.BoostGauge < 10)
        {
            stat.BoostGauge += 2.0f * Time.deltaTime;
        }
    }

    public void Boost()
    {
        stat.Speed = origSpeed * 2;
        stat.BoostGauge -= 4.0f * Time.deltaTime;
    }

    public void Jump()
    {
        rig.AddForce(Vector3.up * jumpForce);
    }

}
