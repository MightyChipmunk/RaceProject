using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffect : MonoBehaviour
{
    float time = 0;
    bool scorePlus = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnScorePlus += IsScorePlus;
    }

    // Update is called once per frame
    void Update()
    {
        //if (scorePlus)
        //{
        //    time += Time.deltaTime;
        //    Anim();
        //}
    }
    //void Anim()
    //{
    //    if (Time.timeScale > 0)
    //    {
    //        if (time < 0.3f)
    //        {
    //            transform.localScale += Vector3.one / 100;
    //        }
    //        else if (time >= 0.3f && time <0.6f) 
    //        {
    //            transform.localScale -= Vector3.one / 100;
    //        }
    //        else
    //        {
    //            transform.localScale = Vector3.one;
    //        }
    //    }
    //}

    void IsScorePlus(object sender, System.EventArgs e)
    {
        //StartCoroutine("TimePlus");
        transform.localScale = Vector3.zero;
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic));
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
        GameManager.Instance.OnScorePlus -= IsScorePlus;
    }
}
