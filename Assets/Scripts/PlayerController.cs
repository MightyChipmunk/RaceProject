using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotSpeed = 80.0f;
    public float speed = 10.0f;
    float lerp = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        Vector3 dir = transform.rotation * Vector3.forward;

        if (lerp >= 0)
            transform.eulerAngles += x * rotSpeed * Time.deltaTime * Vector3.up;
        else
            transform.eulerAngles -= x * rotSpeed * Time.deltaTime * Vector3.up;

        if (Input.GetKey(KeyCode.W))
        {
            lerp = Mathf.Lerp(lerp, speed, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            lerp = Mathf.Lerp(lerp, -speed / 3, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }
        else 
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime);
            transform.position += lerp * dir * Time.deltaTime;
        }
    }
}
