using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIntOut : MonoBehaviour
{
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time < 0.6f)
            GetComponent<Text>().color = new Color(1, 1, 1, 1f - time / 0.6f);
        else if (time >= 0.6f && time < 1.2f)
            GetComponent<Text>().color = new Color(1, 1, 1, (time - 0.6f) / 0.6f);
        else
            time = 0;
    }
}
