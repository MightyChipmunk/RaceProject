using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Turn : MonoBehaviour
{
    GameObject text;
    private void Start()
    {
        text = transform.Find("Text").gameObject;
    }
    private void Update()
    {
        text.transform.eulerAngles = new Vector3(0, 0, 0);
    }
    private void OnMouseOver()
    {

        transform.Rotate(0, 0.4f, 0);
        text.SetActive(true);
    }
 
    private void OnMouseExit()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        text.SetActive(false);
    }

}