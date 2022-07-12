using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject root = GameObject.Find("Managers");
        DontDestroyOnLoad(root);
    }

}
