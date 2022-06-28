using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotSpeed = 80.0f;
    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dir = transform.rotation * Vector3.forward;

        transform.eulerAngles += x * rotSpeed * Time.deltaTime * Vector3.up;
        transform.position += y * speed * Time.deltaTime * dir;
    }
}
