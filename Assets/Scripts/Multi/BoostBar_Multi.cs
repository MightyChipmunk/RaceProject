using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar_Multi : MonoBehaviour
{
    PlayerStat stat;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player2").GetComponent<PlayerStat>();
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = stat.BoostGauge / 10;
    }
}
