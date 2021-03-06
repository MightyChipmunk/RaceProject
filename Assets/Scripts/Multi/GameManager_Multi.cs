using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Multi : MonoBehaviour
{
    public static GameManager_Multi Instance;
    // 차량이 한바퀴 돌때마다 이벤트
    public event EventHandler OnLapEnd;
    public event EventHandler OnScorePlus;
    public event EventHandler OnGameEnd;

    public int Choosed { get; set; }
    public bool IsMulti { get; set; }
    public int Choosed_2 { get; set; }

    // 스코어 및 시간 관리
    public int LastScore { get; private set; }
    int score = 0;
    public int Score 
    {
        set
        {
            if (score == value) return;

            LastScore = value - score;
            score = value;
            if (OnScorePlus != null && value != 0)
                OnScorePlus.Invoke(this, EventArgs.Empty);
        }
        get { return score; }
    }

    float lapTime = 90;
    public float LapTime
    {
        set
        {
            if (lapTime == value) return;
            lapTime = value;
            if (lapTime <= 0 && OnGameEnd != null)
                OnGameEnd.Invoke(this, EventArgs.Empty);
        }
        get { return lapTime; }
    }

    float lapCount = 0;
    public float LapCount
    {
        set
        {
            if (lapCount == value) return;
            lapCount = value;
            if (value != 0)
            {
                if (OnLapEnd != null)
                    OnLapEnd.Invoke(this, EventArgs.Empty);
                Score += 5000;
            }
        }
        get { return lapCount; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        OnGameEnd = null;
        OnLapEnd = null;
        OnScorePlus = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
