using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// based on tactic move. Uses the mouse and raycasts as input to tell the character where to go.
public class PlayerMove : TacticsMove
{
    GameObject Enemy;
    public Transform healthbar;
    public Slider healthfill;
    public float healthbaroffsetY = 2;
    // Use this for initialization
    void Start()
    {
        Init();

        maxhp = 100;
        hp = maxhp;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);
        if (hp <= 0)
        {
            SceneManager.LoadScene(1);

        }
        if (!turn)
        {
            myhitbox.SetActive(false);
            return;
        }
       
        if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
            myhitbox.SetActive(true);

        }


    }
    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "tile")
                {
                    tile t = hit.collider.GetComponent<tile>();
                    if (t.selectable)
                    {
                        MovetoTile(t);

                    }
                }

            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp = hp - damage;

        healthfill.value = hp / maxhp;


    }
    private void PlaceHealthbar()
        {
        Vector3 CurrentPos = transform.position;
        healthbar.position = new Vector3(CurrentPos.x, CurrentPos.y + healthbaroffsetY, CurrentPos.z);
        

        }


}
