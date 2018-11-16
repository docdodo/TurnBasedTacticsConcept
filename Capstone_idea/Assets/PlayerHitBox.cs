using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerHitBox : MonoBehaviour
{
    public GameObject NPC1;
    public int damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            Debug.Log("hitenemy");
            NPC1.GetComponent<NPCMove>().TakeDamage(damage);
        }
    }
}
