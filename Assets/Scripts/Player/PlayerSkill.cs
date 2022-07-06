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
    }

    public void Boost()
    {
        stat.speed = origSpeed * 2;
    }

    public void Jump()
    {
        rig.AddForce(Vector3.up * jumpForce);
    }

}
