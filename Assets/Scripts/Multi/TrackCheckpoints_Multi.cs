using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints_Multi : MonoBehaviour
{
    //�̺�Ʈ ����� ���â
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;


    private List<CheckpointSingle_Multi> checkpointSingleList;
    private int nextCheckpointSingleIndex;
    public int NextCheckpointSingleIndex { get { return nextCheckpointSingleIndex; } }


    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        //üũ����Ʈ ������� ����������
        checkpointSingleList = new List<CheckpointSingle_Multi>();


        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle_Multi checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle_Multi>();

            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;
    }


    //������� 0 1 2 3 ������ �������Բ�
    public bool PlayerThroughCheckpoint(CheckpointSingle_Multi checkpointSingle)
    {

        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            //������ �����ϱ� üũ����Ʈ �ٵ��� �����Ͽ��� �ٽ� 0 1 2 �� ����
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;

            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);

            return true;
        }
        else
        {
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            return false;
        }
    }
}