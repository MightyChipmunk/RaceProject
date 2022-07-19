using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count_Multi : MonoBehaviour
{
    // 랩타임 표시 텍스트
    Text lapTime;
    // 랩타임 표시를 위한 스트링 변수
    string lapS = "";

    Text score;
    string scoreS = "";

    Text scorePlus;

    Text count;
    PlayerController pc;
    bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player2").GetComponent<PlayerController>();
        count = gameObject.GetComponent<Text>();
        lapTime = transform.Find("LapTime").GetComponent<Text>();
        score = transform.Find("Score").GetComponent<Text>();
        scorePlus = transform.Find("ScorePlus").GetComponent<Text>();
        StartCoroutine("CountDown");

        GameManager_Multi.Instance.OnScorePlus += PrintScorePlus;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            // 랩타임 표시
            if (GameManager_Multi.Instance.LapTime >= 0)
                GameManager_Multi.Instance.LapTime -= Time.deltaTime;

            if (GameManager_Multi.Instance.LapTime < 0)
                lapTime.text = "0:00.00";
            else if ((GameManager_Multi.Instance.LapTime % 1).ToString().Length <= 4)
            {

            }
            else if (GameManager_Multi.Instance.LapTime % 60 < 10)
            {
                lapS = ((int)(GameManager_Multi.Instance.LapTime / 60)).ToString() + ":0" + ((int)(GameManager_Multi.Instance.LapTime % 60)).ToString() + "." + (GameManager_Multi.Instance.LapTime % 1).ToString().Substring(2, 2);
            }
            else
            {
                lapS = ((int)(GameManager_Multi.Instance.LapTime / 60)).ToString() + ":" + ((int)(GameManager_Multi.Instance.LapTime % 60)).ToString() + "." + (GameManager_Multi.Instance.LapTime % 1).ToString().Substring(2, 2);
            }
            lapTime.text = lapS;

            // 스코어 표시
            scoreS = GameManager_Multi.Instance.Score.ToString();
            score.text = scoreS;
        }

    }

    void PrintScorePlus(object sender, System.EventArgs e)
    {
        StartCoroutine("ScorePlus");
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
        pc.CanMove = true;
        yield return new WaitForSeconds(1.0f);
        count.enabled = false;
    }

    IEnumerator ScorePlus()
    {
        scorePlus.text = "+ " + GameManager_Multi.Instance.LastScore.ToString();
        yield return new WaitForSeconds(1.0f);
        scorePlus.text = "";
    }

    private void OnDestroy()
    {
        GameManager_Multi.Instance.OnScorePlus -= PrintScorePlus;
    }
}
