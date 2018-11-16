using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Hitbox : MonoBehaviour
{
    public GameObject player;
    public int damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hit");
            player.GetComponent<PlayerMove>().TakeDamage(damage);
        }
    }
}

