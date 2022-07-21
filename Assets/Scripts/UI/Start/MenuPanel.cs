using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{ 
    [SerializeField] GameObject goFirst = null;

    //public void BtnPlay()
    //{
    //    goFirst.SetActive(true);
    //    this.gameObject.SetActive(false);
    //}

    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            goFirst.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}

