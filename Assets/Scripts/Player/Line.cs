using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    PlayerController pc;
    TrailRenderer trail;
    GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();
        trail = transform.Find("SkidMark").GetComponent<TrailRenderer>();
        particle = transform.Find("DriftParticle").gameObject;

        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (pc.IsDrift)
        {
            trail.emitting = true;
            particle.SetActive(true);
        }
        else
        {
            trail.emitting = false;
            particle.SetActive(false);
        }
    }
}