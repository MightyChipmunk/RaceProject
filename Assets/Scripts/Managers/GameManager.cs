using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // ������ �ѹ��� �������� �̺�Ʈ
    public event EventHandler OnLapEnd;

    // ���ھ� �� �ð� ����
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
