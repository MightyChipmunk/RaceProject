using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    //이벤트 만들기 경고창
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;


    private List<CheckpointSingle> checkpointSingleList;
    private int nextCheckpointSingleIndex;


    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        //체크포인트 순서대로 지나가야함
        checkpointSingleList = new List<CheckpointSingle>();


        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;
    }


    //순서대로 0 1 2 3 식으로 지나가게끔
    public bool PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {

        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            Debug.Log("Correct");
            //오류값 수정하기 체크포인트 다돌고 다음턴에서 다시 0 1 2 로 시작
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