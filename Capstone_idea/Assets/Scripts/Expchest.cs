using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expchest : MonoBehaviour {

    public GameObject player;
    bool treasure;
    public float getgold;


    private void Awake()
    {
        player = GameObject.Find("Player");
        treasure = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hit");
            if (treasure == false)
            {
                player.GetComponent<PlayerMove>().GetExp(getgold);
                treasure = true;
            }
        }
    }
}
