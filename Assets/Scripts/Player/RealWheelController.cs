using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWheelController : MonoBehaviour
{
    PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(pc.Lerp, 0, 0);
    }
}
