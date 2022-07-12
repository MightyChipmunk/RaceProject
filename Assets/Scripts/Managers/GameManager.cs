using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // 차량이 한바퀴 돌때마다 이벤트
    public event EventHandler OnLapEnd;

    // 스코어 및 시간 관리
    public int Score { get; set; }
    public float LapTime { get; set; }

    float lapCount = 0;
    public float LapCount
    {
        set
        {
            if (lapCount == value) return;
            lapCount = value;
            if (OnLapEnd != null)
                OnLapEnd.Invoke(this, EventArgs.Empty);
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
