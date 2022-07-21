using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    UnityEngine.UI.Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<UnityEngine.UI.Outline>();

    }

    private void OnMouseOver()
    {
       
        outline.enabled = true;

    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
