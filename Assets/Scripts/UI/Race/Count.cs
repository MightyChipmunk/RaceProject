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

    Text scorePlus;

    Text count;
    PlayerController pc;
    bool isStart = false;
    public bool IsStart { get { return isStart; } set { isStart = value; } }

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        count = gameObject.GetComponent<Text>();
        lapTime = transform.Find("LapTime").GetComponent<Text>();
        score = transform.Find("Score").GetComponent<Text>();
        scorePlus = transform.Find("ScorePlus").GetComponent<Text>();
        StartCoroutine("CountDown");

        GameManager.Instance.OnScorePlus += PrintScorePlus;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            // 랩타임 표시
            if (GameManager.Instance.LapTime >= 0)
                GameManager.Instance.LapTime -= Time.deltaTime;

            if (GameManager.Instance.LapTime < 0)
                lapTime.text = "0:00.00";
            else if ((GameManager.Instance.LapTime % 1).ToString().Length <= 4)
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
        scorePlus.text = "+ " + GameManager.Instance.LastScore.ToString();
        yield return new WaitForSeconds(1.0f);
        scorePlus.text = "";
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnScorePlus -= PrintScorePlus;
    }
}
