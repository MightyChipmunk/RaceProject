using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    PlayerController pc;
    public GameObject Factory;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();

        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.isDrift)
        {
            GameObject plane = Instantiate(Factory);

            plane.transform.position = transform.position - Vector3.up * 0.1f;
            plane.transform.eulerAngles = transform.parent.eulerAngles;

            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}