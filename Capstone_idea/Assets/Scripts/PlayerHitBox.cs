using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerHitBox : MonoBehaviour
{
    public GameObject NPC;
    public GameObject Player;
    public float Totaldamage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            Debug.Log("hitenemy");
            NPC = other.gameObject;
            
            Totaldamage = Player.GetComponent<PlayerMove>().MeleeDamage;
            NPC.GetComponent<NPCMove>().TakeDamage(Totaldamage);
        }
    }
}
