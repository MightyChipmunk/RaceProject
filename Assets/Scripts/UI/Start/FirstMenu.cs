using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMenu : MonoBehaviour
{
    [SerializeField] GameObject goSecond = null;

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void BtnPlay1()
    {
        goSecond.SetActive(true);
        this.gameObject.SetActive(false);

        GameManager.Instance.IsMulti = false;
    }
    public void BtnPlay2()
    {
        goSecond.SetActive(true);
        this.gameObject.SetActive(false);

        GameManager.Instance.IsMulti = true;
    }

}
