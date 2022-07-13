using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMenu : MonoBehaviour
{ 
    [SerializeField] GameObject thirdMenu = null;

    public void BtnPlay1()
    {
        GameManager.Instance.Choosed = 1;
        thirdMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay2()
    {
        GameManager.Instance.Choosed = 2;
        thirdMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay3()
    {
        GameManager.Instance.Choosed = 2;
        thirdMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
