using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    GameObject Car;
    [SerializeField]
    GameObject Taxi;
    [SerializeField]
    GameObject Coupe;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance.Choosed == 1)
        {
            player = Instantiate(Car);
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            player.name = "Player";
            player.transform.SetParent(transform);
        }
        else if (GameManager.Instance.Choosed == 2)
        {
            player = Instantiate(Taxi);
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            player.name = "Player";
            player.transform.SetParent(transform);
        }
        else if (GameManager.Instance.Choosed == 3)
        {
            player = Instantiate(Coupe);
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            player.name = "Player";
            player.transform.SetParent(transform);
        }

    }
}
