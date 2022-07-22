using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    GameObject gameEnd;
    GameObject scoreEnd;
    GameObject gamePause;
    GameObject title;
    GameObject retry;
    GameObject quit;
    Count count;
    Text endText;
    PlayerController pc;
    PlayerController pc2;
    // Start is called before the first frame update
    void Start()
    {
        gameEnd = transform.Find("GameEnd").gameObject;
        scoreEnd = gameEnd.transform.Find("ScoreEnd").gameObject;
        endText = scoreEnd.GetComponent<Text>();
        gameEnd.SetActive(false);

        gamePause = transform.Find("GamePause").gameObject;
        gamePause.SetActive(false);

        GameManager.Instance.OnGameEnd += EndScreen;
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        if (GameManager.Instance.IsMulti)
        {
            pc2 = GameObject.Find("Player2").GetComponent<PlayerController>();
        }

        count = transform.Find("IngamePanel").transform.Find("Count").GetComponent<Count>();

        title = gameEnd.transform.Find("TitleButton").gameObject;
        retry = gameEnd.transform.Find("RetryButton").gameObject;
        quit = gameEnd.transform.Find("QuitButton").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (InputManager.Instance.Pause && gamePause.activeSelf == false)
        if (InputManager.Instance.Pause && gamePause.activeSelf == false)
        {
            SoundManager.Instance.Stop(SoundManager.Sound.Booster);
            SoundManager.Instance.Stop(SoundManager.Sound.Engine);
            SoundManager.Instance.Stop(SoundManager.Sound.Drift);
            SoundManager.Instance.Pause(SoundManager.Sound.Bgm);
            pc.CanMove = false;
            if (GameManager.Instance.IsMulti)
            {
                SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Booster);
                SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Engine);
                SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift);
                pc2.CanMove = false;
            }
            gamePause.SetActive(true);
            Time.timeScale = 0;
        }
        else if (InputManager.Instance.Pause && gamePause.activeSelf)
        {
            SoundManager.Instance.UnPause(SoundManager.Sound.Bgm);
            if (count.IsStart)
                pc.CanMove = true;
            if (GameManager.Instance.IsMulti && count.IsStart)
            {
                pc2.CanMove = true;
            }
            gamePause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void EndScreen(object sender, System.EventArgs e)
    {
        gameEnd.SetActive(true);
        SoundManager.Instance.Stop(SoundManager.Sound.Booster);
        SoundManager.Instance.Stop(SoundManager.Sound.Engine);
        SoundManager.Instance.Stop(SoundManager.Sound.Drift);
        if (!GameManager.Instance.IsMulti)
        {
            endText.text = "Total Score:\n" + GameManager.Instance.Score.ToString();
            scoreEnd.transform.localScale = Vector3.zero;
            iTween.ScaleTo(scoreEnd, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic));
        }
        else if (GameManager.Instance.IsMulti)
        {
            if (GameManager_Multi.Instance.Score > GameManager.Instance.Score)
            {
                endText.text = "Winner:\nPlayer 2!";
                scoreEnd.transform.localScale = Vector3.zero;
                iTween.ScaleTo(scoreEnd, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic));
            }
            else if (GameManager_Multi.Instance.Score < GameManager.Instance.Score)
            {
                endText.text = "Winner:\nPlayer 1!";
                scoreEnd.transform.localScale = Vector3.zero;
                iTween.ScaleTo(scoreEnd, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic));
            }
            else
            {
                endText.text = "Draw!";
                scoreEnd.transform.localScale = Vector3.zero;
                iTween.ScaleTo(scoreEnd, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic));
            }
        }
        pc.CanMove = false; 
        if (GameManager.Instance.IsMulti)
        {
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Booster);
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Engine);
            SoundManager_Multi.Instance.Stop(SoundManager_Multi.Sound.Drift);
            pc2.CanMove = false;
        }

        title.transform.localPosition = new Vector3(0, -300, 0);
        retry.transform.localPosition = new Vector3(0, -300, 0);
        quit.transform.localPosition = new Vector3(0, -300, 0);
        iTween.MoveTo(title, iTween.Hash("x", 0, "y", -2, "z", 0, "time", 0.8f, "easetype", iTween.EaseType.easeOutCirc, "delay", 0, "islocal", true));
        iTween.MoveTo(retry, iTween.Hash("x", 0, "y", -79, "z", 0, "time", 0.8f, "easetype", iTween.EaseType.easeOutCirc, "delay", 0.3f, "islocal", true));
        iTween.MoveTo(quit, iTween.Hash("x", 0, "y", -156, "z", 0, "time", 0.8f, "easetype", iTween.EaseType.easeOutCirc, "delay", 0.6f, "islocal", true));
    }

    public void Retry()
    {
        Time.timeScale = 1;
        GameManager.Instance.LapTime = 90;
        GameManager.Instance.Score = 0;
        GameManager.Instance.LapCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void Title()
    {
        Time.timeScale = 1;
        GameManager.Instance.LapTime = 90;
        GameManager.Instance.Score = 0;
        GameManager.Instance.LapCount = 0;
        SceneManager.LoadScene("StartScene");
    }

    public void Continue()
    {
        pc.CanMove = true;
        if (GameManager.Instance.IsMulti)
        {
            pc2.CanMove = true;
        }
        SoundManager.Instance.UnPause(SoundManager.Sound.Bgm);
        gamePause.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameEnd -= EndScreen;
    }
}
