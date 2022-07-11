using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    float speed = 30;
    public float Speed { get { return speed; } set { speed = value; } }
    float rotSpeed = 80;
    public float RotSpeed { get { return rotSpeed; } set { rotSpeed = value; } }
    float accelPower = 3;
    public float AccelPower { get { return accelPower; } set { accelPower = value;} }
    float brakePower = 5;
    public float BrakePower { get { return brakePower; } set { } }
    float boostGauge = 10;
    public float BoostGauge { get { return boostGauge; } set { boostGauge = value;} }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
