using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{ 
    [SerializeField] GameObject gosecond = null;

    public void BtnPlay()
    {
        gosecond.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

