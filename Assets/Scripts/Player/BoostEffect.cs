using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    PlayerController pc;
    GameObject booster;
    GameObject idle;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();
        booster = transform.Find("BoostParticle").gameObject;
        //idle = transform.Find("IdleParticle").gameObject;

        booster.SetActive(false);
        //idle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.IsBooster)
        {
            booster.SetActive(true);
            //idle.SetActive(false);
        }
        else
        {
            booster.SetActive(false);
            //idle.SetActive(true);
        }
    }
}
