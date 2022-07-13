using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameStartButton : MonoBehaviour
{
    public GameObject gameCam;
    public GameObject menuPanel;
    public GameObject ingamePanel;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void GameStart()
    {
        gameCam.SetActive(true);
        menuPanel.SetActive(false);
        ingamePanel.SetActive(true);

    }


}
