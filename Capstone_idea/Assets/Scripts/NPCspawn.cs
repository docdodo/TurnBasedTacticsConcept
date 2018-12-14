using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCspawn : MonoBehaviour {

    public GameObject player;
    public GameObject NPCtospawn;
    public Vector3 Spawnlocation;
    public bool Spawned;
    private void Start()
    {
        Spawned = false;
        player = GameObject.Find("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("spawn");

            if (Spawned == false)
            {
              Instantiate(NPCtospawn, (Spawnlocation), Quaternion.Euler(0, 180, 0));
                Spawned = true;
            }

        }
    }
    public void PlayerDeath()
    {
        Spawned = false;
    }
}