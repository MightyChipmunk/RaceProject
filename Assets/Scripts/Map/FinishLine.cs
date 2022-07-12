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
        // ���� �÷��̾ ��߼��� ������ �ð� �ʱ�ȭ
        // ù��° üũ����Ʈ�� ��߼����� ��¦ �ڿ� �־�ߵ�
        if (other.gameObject.name.Contains("Player") && trackCheckpoints.NextCheckpointSingleIndex == 0)
        {
            GameManager.Instance.LapCount++;
        }
    }
}
