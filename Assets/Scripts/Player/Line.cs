using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    PlayerController pc;
    TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();
        trail = transform.GetChild(1).GetComponent<TrailRenderer>();

        trail.emitting = false;

        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (pc.IsDrift)
        {
            trail.emitting = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            trail.emitting = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}