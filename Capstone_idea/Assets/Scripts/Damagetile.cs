using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagetile : MonoBehaviour
{
    public GameObject player;
    public int damage;



    private void Awake()
    {
        player = GameObject.Find("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("damage");
            player.GetComponent<PlayerMove>().TakeDamage(damage);
        }
    }
}
