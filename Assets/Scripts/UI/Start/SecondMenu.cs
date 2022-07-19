using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondMenu : MonoBehaviour
{ 
    [SerializeField] GameObject thirdMenu = null;
    int cnt = 2;

    Text annouce;

    private void Start()
    {
        if (GameManager.Instance.IsMulti)
        {
            annouce = transform.Find("Announce").GetComponent<Text>();

            annouce.text = "Choose Player1 Car";
        }
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
                annouce.text = "Choose Player2 Car";
            }
            else if (cnt == 1)
                GameManager.Instance.Choosed_2 = 1;

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
                annouce.text = "Choose Player2 Car";
            }
            else if (cnt == 1)
                GameManager.Instance.Choosed_2 = 2;

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
                annouce.text = "Choose Player2 Car";
            }
            else if (cnt == 1)
                GameManager.Instance.Choosed_2 = 3;

            cnt--;
        }
    }

    private void Update()
    {
        if (cnt == 0)
        {
            thirdMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
