using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.Pause)
        {
            GameManager.Instance.Choosed = 0;
            GameManager.Instance.Choosed_2 = 0;
            GameManager.Instance.IsMulti = false;
            SceneManager.LoadScene("StartScene");
        }
    }
}
