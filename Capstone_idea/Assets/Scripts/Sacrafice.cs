using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// npc move uses A* to find the easiest path to the player.
public class Sacrafice : TacticsMove
{
    GameObject target;
    public float hp;
    public float maxhp;
    public bool dead = false;
    // Use this for initialization
    void Awake()
    {
        Init();
        dead = true;
        maxhp = 0.0f;
        hp = maxhp;
        move = 5;   //Enemies can move 1 more space so they can catch up to the player.
        turn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (dead == false && hp <= 0)
        {

            GetComponent<Renderer>().material.color = Color.black;
            dead = true;
        }
        if (!turn)
        {
           
            return;
        }

        if (dead == false)
        {
            if (!moving)
            {
                FindNearestTarget();
                CalculatePath();
                FindSelectableTiles();
                actualtargettile.target = true;
            }
            else
            {
                Move();
                myhitbox.SetActive(true);
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
            TurnManager.EndTurn();

        }
    }

    void CalculatePath()
    {
        tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }
    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);
            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }
        target = nearest;
    }
    public void TakeDamage(float damage)
    {
        hp = hp - damage;




    }


}
