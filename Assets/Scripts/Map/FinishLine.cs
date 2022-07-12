using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    //public event EventHandler OnLapEnd;

    private void Start()
    {
        trackCheckpoints = transform.parent.transform.parent.transform.parent.GetComponent<TrackCheckpoints>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // 만약 플레이어가 출발선에 닿으면 시간 초기화
        // 첫번째 체크포인트가 출발선보다 살짝 뒤에 있어야됨
        if (other.gameObject.name.Contains("Player") && trackCheckpoints.NextCheckpointSingleIndex == 0)
        {
            GameManager.Instance.LapCount++;
        }
    }
}
