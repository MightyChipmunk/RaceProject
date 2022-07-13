using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // ������ �ѹ��� �������� �̺�Ʈ
    public event EventHandler OnLapEnd;
    public event EventHandler OnScorePlus;
    public event EventHandler OnGameEnd;

    // ���ھ� �� �ð� ����
    public int LastScore { get; private set; }
    int score = 0;
    public int Score 
    {
        set
        {
            if (score == value) return;
            LastScore = value - score;
            score = value;
            if (OnScorePlus != null)
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
            if (OnLapEnd != null)
                OnLapEnd.Invoke(this, EventArgs.Empty);
            Score += 5000;
        }
        get { return lapCount; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
