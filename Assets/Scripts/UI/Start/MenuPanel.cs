using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{ 
    [SerializeField] GameObject goFirst = null;

    public GameObject title;

    private void Start()
    {
        gameObject.SetActive(true);

        title.transform.localScale = Vector3.zero;

        iTween.ScaleTo(title, iTween.Hash("x", 1, "y", 1, "z", 1,  "time", 0.5f, "easetype", iTween.EaseType.easeOutBack, "delay", 0.5f));

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

