using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdMenu : MonoBehaviour
{
    private void Start()
    {
        //gameObject.SetActive(false);
    }
    public void BtnPlay1()
    {
        if (!GameManager.Instance.IsMulti)
            SceneManager.LoadScene("RaceScene 1");
        else if (GameManager.Instance.IsMulti)
            SceneManager.LoadScene("RaceScene 1_Multi");
    }
    public void BtnPlay2()
    {
        if (!GameManager.Instance.IsMulti)
            SceneManager.LoadScene("RaceScene 2");
        else if (GameManager.Instance.IsMulti)
            SceneManager.LoadScene("RaceScene 2_Multi");
    }
    public void BtnPlay3()
    {
        if (!GameManager.Instance.IsMulti)
            SceneManager.LoadScene("RaceScene 3");
        else if (GameManager.Instance.IsMulti)
            SceneManager.LoadScene("RaceScene 3_Multi");
    }
}
