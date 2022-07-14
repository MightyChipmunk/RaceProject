using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    GameObject gameEnd;
    GameObject gamePause;
    Text endText;
    PlayerController pc;
    AudioListener listener;
    // Start is called before the first frame update
    void Start()
    {
        gameEnd = transform.Find("GameEnd").gameObject;
        endText = gameEnd.transform.Find("ScoreEnd").GetComponent<Text>();
        gameEnd.SetActive(false);

        gamePause = transform.Find("GamePause").gameObject;
        gamePause.SetActive(false);

        GameManager.Instance.OnGameEnd += EndScreen;
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        listener = GameObject.Find("Camera").GetComponent<AudioListener>();
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
            gamePause.SetActive(true);
            Time.timeScale = 0;
        }
        else if (InputManager.Instance.Pause && gamePause.activeSelf)
        {
            //listener.enabled = true;
            SoundManager.Instance.UnPause(SoundManager.Sound.Bgm);
            pc.CanMove = true;
            gamePause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void EndScreen(object sender, System.EventArgs e)
    {
        gameEnd.SetActive(true);
        //listener.enabled = false;
        SoundManager.Instance.Stop(SoundManager.Sound.Booster);
        SoundManager.Instance.Stop(SoundManager.Sound.Engine);
        SoundManager.Instance.Stop(SoundManager.Sound.Drift);
        endText.text = "Total Score:\n" + GameManager.Instance.Score.ToString();
        pc.CanMove = false;
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
        listener.enabled = true;
        SoundManager.Instance.UnPause(SoundManager.Sound.Bgm);
        gamePause.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameEnd -= EndScreen;
    }
}
