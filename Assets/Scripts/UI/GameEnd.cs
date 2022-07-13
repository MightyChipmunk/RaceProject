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
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.Pause && gamePause.activeSelf == false)
        {
            gamePause.SetActive(true);
            Time.timeScale = 0;
        }
        else if (InputManager.Instance.Pause && gamePause.activeSelf)
        {
            gamePause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void EndScreen(object sender, System.EventArgs e)
    {
        gameEnd.SetActive(true);
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

    public void Continue()
    {
        gamePause.SetActive(false);
        Time.timeScale = 1;
    }
}
