using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdMenu : MonoBehaviour
{
    public void BtnPlay1()
    {
        SceneManager.LoadScene("RaceScene 1");
    }
    public void BtnPlay2()
    {
        SceneManager.LoadScene("RaceScene 2");
    }
    public void BtnPlay3()
    {
        SceneManager.LoadScene("RaceScene 3");
    }
}
