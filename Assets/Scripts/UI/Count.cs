using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    // 랩타임 표시 텍스트
    Text lapTime;
    // 랩타임 표시를 위한 스트링 변수
    string lapS = "";

    Text score;
    string scoreS = "";

    Text count;
    PlayerController pc;
    bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        count = gameObject.GetComponent<Text>();
        lapTime = transform.GetChild(0).GetComponent<Text>();
        score = transform.GetChild(1).GetComponent<Text>();
        StartCoroutine("CountDown");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            // 랩타임 표시
            if (GameManager.Instance.LapCount < 5)
                GameManager.Instance.LapTime += Time.deltaTime;
            if ((GameManager.Instance.LapTime % 1).ToString().Length < 2)
            {

            }
            else if (GameManager.Instance.LapTime % 60 < 10)
            {
                lapS = ((int)(GameManager.Instance.LapTime / 60)).ToString() + ":0" + ((int)(GameManager.Instance.LapTime % 60)).ToString() + "." + (GameManager.Instance.LapTime % 1).ToString().Substring(2, 2);
            }
            else
            {
                lapS = ((int)(GameManager.Instance.LapTime / 60)).ToString() + ":" + ((int)(GameManager.Instance.LapTime % 60)).ToString() + "." + (GameManager.Instance.LapTime % 1).ToString().Substring(2, 2);
            }
            lapTime.text = lapS;

            // 스코어 표시
            scoreS = GameManager.Instance.Score.ToString();
            score.text = scoreS;
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
        GameManager.Instance.LapTime = 0;
        pc.CanMove = true;
        yield return new WaitForSeconds(1.0f);
        count.enabled = false;
    }
}
