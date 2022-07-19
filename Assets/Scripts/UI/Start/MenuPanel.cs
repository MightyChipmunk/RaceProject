using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{ 
    [SerializeField] GameObject goFirst = null;

    public void BtnPlay()
    {
        goFirst.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

