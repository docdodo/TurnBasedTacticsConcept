using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// npc move uses A* to find the easiest path to the player.
public class Bossscript : TacticsMove
{
    GameObject target;
    public GameObject Player;
    public GameObject walls;
    public GameObject myspawn;
    public float hp;
    public float maxhp;
    public bool dead = false;
    public float expgainonkill;
    // Use this for initialization
    void Awake()
    {
        Init();

        Player = GameObject.Find("Player");
        hp = maxhp;
        move = 5;   //Enemies can move 1 more space so they can catch up to the player.
        turn = false;
        walls = GameObject.Find("Bosswalls");
        myspawn = GameObject.Find("Bossspawn");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);


        if (!turn)
        {
            myhitbox.SetActive(false);
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
        if (hp <= 0)
        {



            Player.GetComponent<PlayerMove>().GetExp(expgainonkill);
            TurnManager.RemoveUnit(this);
            Destroy(walls.gameObject);
            Destroy(myspawn);
            Destroy(this.gameObject);

        }



    }

    public void Die()
    {
        RemoveSelectableTiles();
        TurnManager.EndTurn();

        TurnManager.RemoveUnit(this);
      
        Destroy(this.gameObject);
    }

}
