using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// based on tactic move. Uses the mouse and raycasts as input to tell the character where to go.
public class PlayerMove : TacticsMove
{
    GameObject Enemy;
   
    // Use this for initialization
    void Start ()
    {
        Init();
      
        maxhp = 100;
        hp = maxhp;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawRay(transform.position, transform.forward);
        if (hp <=0)
        {
            SceneManager.LoadScene(1);

        }
        if(!turn)
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
    

    

}
