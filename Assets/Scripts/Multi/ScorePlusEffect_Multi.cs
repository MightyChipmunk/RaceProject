using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePlusEffect_Multi : MonoBehaviour
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

    public void Anim()
    {
        if (Time.timeScale > 0)
        {
            if (time < 0.6f)
            {
                GetComponent<Text>().color = new Color(1, 1, 1, 1f - time / 0.6f);
                transform.localScale += Vector3.one / 200;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }

    void IsScorePlus(object sender, System.EventArgs e)
    {
        StartCoroutine("ScorePlus");
    }

    IEnumerator ScorePlus()
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
