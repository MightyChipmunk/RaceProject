using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondMenu : MonoBehaviour
{ 
    [SerializeField] 
    GameObject thirdMenu = null;
    [SerializeField]
    GameObject player1;
    [SerializeField]
    GameObject player2;
    [SerializeField]
    GameObject next;
    [SerializeField]
    GameObject reselect;

    int cnt = 2;

    Text annouce;

    private void Start()
    {
        //gameObject.SetActive(false);

        if (GameManager.Instance.IsMulti)
        {
            annouce = transform.Find("Announce").GetComponent<Text>();

            annouce.text = "Choose Player1 Car";
        }

        player1.GetComponent<Text>().enabled = false;
        player2.GetComponent<Text>().enabled = false;
        reselect.SetActive(false);
        next.SetActive(false);
    }

    public void BtnPlay1()
    {
        if (!GameManager.Instance.IsMulti)
        {
            GameManager.Instance.Choosed = 1;
            thirdMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.IsMulti)
        {
            if (cnt == 2)
            {
                GameManager.Instance.Choosed = 1;
                player1.GetComponent<Text>().enabled = true;
                player1.GetComponent<RectTransform>().localPosition = new Vector3(-250, -195, 0);
                annouce.text = "Choose Player2 Car";
            }
            else if (cnt == 1)
            {
                GameManager.Instance.Choosed_2 = 1;
                player2.GetComponent<Text>().enabled = true;
                player2.GetComponent<RectTransform>().localPosition = new Vector3(-250, -195, 0);
            }

            cnt--;
        }
    }

    public void BtnPlay2()
    {
        if (!GameManager.Instance.IsMulti)
        {
            GameManager.Instance.Choosed = 2;
            thirdMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.IsMulti)
        {
            if (cnt == 2)
            {
                GameManager.Instance.Choosed = 2;
                player1.GetComponent<Text>().enabled = true;
                player1.GetComponent<RectTransform>().localPosition = new Vector3(0, -195, 0);
                annouce.text = "Choose Player2 Car";
            }
            else if (cnt == 1)
            {
                GameManager.Instance.Choosed_2 = 2;
                player2.GetComponent<Text>().enabled = true;
                player2.GetComponent<RectTransform>().localPosition = new Vector3(0, -195, 0);
            }

            cnt--;
        }
    }

    public void BtnPlay3()
    {
        if (!GameManager.Instance.IsMulti)
        {
            GameManager.Instance.Choosed = 3;
            thirdMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.IsMulti)
        {
            if (cnt == 2)
            {
                GameManager.Instance.Choosed = 3;
                player1.GetComponent<Text>().enabled = true;
                player1.GetComponent<RectTransform>().localPosition = new Vector3(250, -195, 0);
                annouce.text = "Choose Player2 Car";
            }
            else if (cnt == 1)
            {
                GameManager.Instance.Choosed_2 = 3;
                player2.GetComponent<Text>().enabled = true;
                player2.GetComponent<RectTransform>().localPosition = new Vector3(250, -195, 0);
            }

            cnt--;
        }
    }

    public void BtnNext()
    {
        thirdMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnReselect()
    {
        GameManager.Instance.Choosed = 0;
        GameManager.Instance.Choosed_2 = 0;
        cnt = 2;
        player1.GetComponent<Text>().enabled = false;
        player2.GetComponent<Text>().enabled = false;
        next.SetActive(false);
        reselect.SetActive(false);
        annouce.text = "Choose Player1 Car";
    }

    private void Update()
    {
        if (cnt == 0)
        {
            next.SetActive(true);
            reselect.SetActive(true);
        }
    }
}
