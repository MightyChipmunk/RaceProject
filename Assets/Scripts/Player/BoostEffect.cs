using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerController>();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.IsBooster)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
