using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints_Multi : MonoBehaviour
{
    //이벤트 만들기 경고창
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;


    private List<CheckpointSingle_Multi> checkpointSingleList;
    private int nextCheckpointSingleIndex;
    public int NextCheckpointSingleIndex { get { return nextCheckpointSingleIndex; } }


    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        //체크포인트 순서대로 지나가야함
        checkpointSingleList = new List<CheckpointSingle_Multi>();


        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle_Multi checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle_Multi>();

            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;
    }


    //순서대로 0 1 2 3 식으로 지나가게끔
    public bool PlayerThroughCheckpoint(CheckpointSingle_Multi checkpointSingle)
    {

        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            //오류값 수정하기 체크포인트 다돌고 다음턴에서 다시 0 1 2 로 시작
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