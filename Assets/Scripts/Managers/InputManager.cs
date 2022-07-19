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


    float horizon2;
    public float Horizon2 { get { return horizon2; } }

    bool accel2;
    public bool Accel2 { get { return accel2; } }

    bool brake2;
    public bool Brake2 { get { return brake2; } }

    bool drift2;
    public bool Drift2 { get { return drift2; } }

    bool driftEnd2;
    public bool DriftEnd2 { get { return driftEnd2; } }

    bool boost2;
    public bool Boost2 { get { return boost2; } }

    bool boostEnd2;
    public bool BoostEnd2 { get { return boostEnd2; } }

    bool jump2;
    public bool Jump2 { get { return jump2; } }


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
        horizon2 = Input.GetAxis("Horizontal2");
        accel2 = Input.GetKey(KeyCode.P);
        brake2 = Input.GetKey(KeyCode.Semicolon);
        drift2 = Input.GetKey(KeyCode.KeypadPlus);
        driftEnd2 = Input.GetKeyUp(KeyCode.KeypadPlus);
        boost2 = Input.GetKey(KeyCode.KeypadEnter);
        boostEnd2 = Input.GetKeyUp(KeyCode.KeypadEnter);
        jump2 = Input.GetKeyDown(KeyCode.Keypad0);
    }
}