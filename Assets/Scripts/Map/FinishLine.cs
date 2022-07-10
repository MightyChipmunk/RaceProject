using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    Count count;
    int cnt = 0;

    private void Start()
    {
        count = GameObject.Find("Canvas").transform.Find("IngamePanel").transform.Find("Count").GetComponent<Count>(); ;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player") && cnt != 0)
        {
            count.CurrentTime = 0;
        }
        else
            cnt++;
    }
}
