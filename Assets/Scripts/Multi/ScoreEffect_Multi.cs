using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffect_Multi : MonoBehaviour
{
    float time = 0;
    bool scorePlus = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager_Multi.Instance.OnScorePlus += IsScorePlus;
    }

    // Update is called once per frame
    void Update()
    {
        if (scorePlus)
        {
            time += Time.deltaTime;
            Anim();
        }
    }
    void Anim()
    {
        if (time < 0.3f)
        {
            transform.localScale += Vector3.one / 100;
        }
        else if (time >= 0.3f && time <0.6f) 
        {
            transform.localScale -= Vector3.one / 100;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    void IsScorePlus(object sender, System.EventArgs e)
    {
        StartCoroutine("TimePlus");
    }

    IEnumerator TimePlus()
    {
        scorePlus = true;
        yield return new WaitForSeconds(0.6f);
        scorePlus = false;
        time = 0;
    }

    private void OnDestroy()
    {
        GameManager_Multi.Instance.OnScorePlus -= IsScorePlus;
    }
}
