using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    float horizon;
    public float Horizon { get { return horizon; } }
    bool accel;
    public bool Accel { get { return accel; } }
    bool brake;
    public bool Brake { get { return brake; } }
    bool drift;
    public bool Drift { get { return drift; } }
    bool driftEnd;
    public bool DriftEnd { get { return driftEnd; } }
    bool boost;
    public bool Boost { get { return boost; } }
    bool boostEnd;
    public bool BoostEnd { get { return boostEnd; } }
    bool jump;
    public bool Jump { get { return jump; } }
    bool pause;
    public bool Pause { get { return pause; } }

    void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        horizon = Input.GetAxis("Horizontal");
        accel = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        brake = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        drift = Input.GetKey(KeyCode.LeftShift);
        driftEnd = Input.GetKeyUp(KeyCode.LeftShift);
        boost = Input.GetKey(KeyCode.LeftControl);
        boostEnd = Input.GetKeyUp(KeyCode.LeftControl);
        jump = Input.GetKeyDown(KeyCode.Space);
        pause = Input.GetKeyDown(KeyCode.Escape);
    }
}