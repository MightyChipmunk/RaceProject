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
        // 맵에 배치된 Count UI를 찾음
        count = GameObject.Find("Canvas").transform.Find("IngamePanel").transform.Find("Count").GetComponent<Count>(); ;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // 만약 플레이어가 출발선에 닿으면 시간 초기화
        if (other.gameObject.name.Contains("Player") && cnt != 0)
        {
            count.CurrentTime = 0;
        }
        else
            cnt++;
    }
}
