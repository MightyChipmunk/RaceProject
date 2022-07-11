using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    //�̺�Ʈ ����� ���â
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;


    private List<CheckpointSingle> checkpointSingleList;
    private int nextCheckpointSingleIndex;


    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        //üũ����Ʈ ������� ����������
        checkpointSingleList = new List<CheckpointSingle>();


        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;
    }


    //������� 0 1 2 3 ������ �������Բ�
    public bool PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {

        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            Debug.Log("Correct");
            //������ �����ϱ� üũ����Ʈ �ٵ��� �����Ͽ��� �ٽ� 0 1 2 �� ����
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;

            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);

            return true;
        }
        else
        {
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            return false;
        }
    }

}