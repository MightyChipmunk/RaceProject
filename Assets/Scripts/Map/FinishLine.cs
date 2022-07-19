using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private TrackCheckpoints_Multi trackCheckpointsMul;
    //public event EventHandler OnLapEnd;

    private void Start()
    {
        if (!GameManager.Instance.IsMulti)
            trackCheckpoints = transform.parent.transform.parent.transform.parent.GetComponent<TrackCheckpoints>();
        else if (GameManager.Instance.IsMulti)
        {
            trackCheckpoints = GameObject.Find("Player1CheckPoint").GetComponent<TrackCheckpoints>();
            trackCheckpointsMul = GameObject.Find("Player2CheckPoint").GetComponent<TrackCheckpoints_Multi>();
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // ���� �÷��̾ ��߼��� ������ �ð� �ʱ�ȭ
        // ù��° üũ����Ʈ�� ��߼����� ��¦ �ڿ� �־�ߵ�
        if (!GameManager.Instance.IsMulti)
        {
            if (other.gameObject.name.Contains("Player") && trackCheckpoints.NextCheckpointSingleIndex == 0)
            {
                GameManager.Instance.LapCount++;
            }
        }
        else if (GameManager.Instance.IsMulti)
        {
            if (other.gameObject.name == "Player" && trackCheckpoints.NextCheckpointSingleIndex == 0)
            {
                GameManager.Instance.LapCount++;
            }
            if (other.gameObject.name == "Player2" && trackCheckpointsMul.NextCheckpointSingleIndex == 0)
            {
                GameManager_Multi.Instance.LapCount++;
            }
        }
    }
}
