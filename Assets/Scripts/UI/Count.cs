using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    // 랩타임 표시 텍스트
    Text lapTime;
    // 랩타임 표시를 위한 스트링 변수
    string s = "";
    // 현재 랩타임
    float currentTime = 0;

    Text count;
    PlayerController pc;
    bool isStart = false;

    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        count = gameObject.GetComponent<Text>();
        lapTime = transform.GetChild(0).GetComponent<Text>();
        StartCoroutine("CountDown");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            currentTime += Time.deltaTime;
            if (currentTime.ToString().Length < 2)
            {

            }
            else if (currentTime % 60 < 10)
            {
                s = ((int)(currentTime / 60)).ToString() + ":0" + ((int)(currentTime % 60)).ToString() + "." + (currentTime % 1).ToString().Substring(2, 2);
            }
            else
            {
                s = ((int)(currentTime / 60)).ToString() + ":" + ((int)(currentTime % 60)).ToString() + "." + (currentTime % 1).ToString().Substring(2, 2);
            }
            lapTime.text = s;
        }


    }

    IEnumerator CountDown()
    {
        count.text = "3";
        yield return new WaitForSeconds(1.0f);
        count.text = "2";
        yield return new WaitForSeconds(1.0f);
        count.text = "1";
        yield return new WaitForSeconds(1.0f);
        count.text = "Go";
        isStart = true;
        currentTime = 0;
        pc.CanMove = true;
        yield return new WaitForSeconds(1.0f);
        count.enabled = false;
    }
}
