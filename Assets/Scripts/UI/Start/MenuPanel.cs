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

        title.transform.localPosition = new Vector3 (-600, 90, 0);

        iTween.MoveTo(title, iTween.Hash("x", 0, "y", 90, "z", 1, "islocal", true, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack, "delay", 0.2f));

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

