using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniMap: MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public GameObject menuPanel;
    public GameObject ingamePanel;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    public void GameStart()
    {
        menuCam.SetActive(false);
        //gameCam.SetActive(true);

        menuPanel.SetActive(false);
        ingamePanel.SetActive(true);

        //player.gameObject.SetActive(true);

    }
}
