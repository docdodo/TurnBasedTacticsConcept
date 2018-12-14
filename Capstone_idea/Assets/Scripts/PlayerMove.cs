using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// based on tactic move. Uses the mouse and raycasts as input to tell the character where to go.
public class PlayerMove : TacticsMove
{
    public GameObject exptext1;
    public GameObject expcosttext1;
   public Text exptext;
   public Text expcosttext;
    public GameObject LevelSystemBack;
    public GameObject UIcanvas;
    public GameObject player;
    public Slider healthSlider;
    public Slider MpSlider;
    public float maxhp;
    public float hp;
    public bool dead = false;
    public float EXP;
    public float EXPCost;
    public float Mp;
    public float MaxMp;
    
    public float MeleeDamage;
    public float MagicDamage;
    public bool LevelSys;
    public GameObject[] Spawners;
    public GameObject[] NPCs;


    public GameObject LightningHbox;
    public GameObject IceHbox;
    public GameObject FireHbox;

    // Use this for initialization
    void Start()
    {
        Init();
        dead = false;
        maxhp = 100;
        hp = maxhp;
        turn = true;
        EXP = 500.0f;
        EXPCost = 100.0f;
        MaxMp = 200;
        Mp = MaxMp;
        MeleeDamage = 20;
        MagicDamage = 20;
        
        LevelSys = false;
        exptext = exptext1.GetComponent<Text>();
        expcosttext = expcosttext1.GetComponent<Text>();
        exptext.text = "Exp : " + EXP;
        expcosttext.text = "Upgrade cost : " + EXPCost;
        healthSlider.maxValue = maxhp;
        healthSlider.value = hp;
        MpSlider.maxValue = MaxMp;
        MpSlider.value = Mp;
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
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
        if (Input.GetKeyDown("f"))
        {
            TakeDamage(20.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(LevelSys==false)
            {
                LevelSystemBack.SetActive(true);
                UIcanvas.SetActive(false);
                LevelSys = true;
            }
            else
            {
                LevelSystemBack.SetActive(false);
                LevelSys = false;
                UIcanvas.SetActive(true);
            }
        }
        if (!moving)
        {
            myhitbox.SetActive(false);
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

    public void TakeDamage(float damage)
    {
        hp = hp - damage;
        healthSlider.value = hp;
        if (hp <= 0)
        {

            NPCs = GameObject.FindGameObjectsWithTag("NPCfinder");
            Spawners = GameObject.FindGameObjectsWithTag("Spawner");
            RemoveSelectableTiles();
            transform.position = new Vector3(7.5f,1.4f,-15.5f);
            hp = maxhp;
            healthSlider.value = hp;
            Mp = MaxMp;
            MpSlider.value = Mp;
            foreach (GameObject foundOne in Spawners)
            {
                GameObject objectMain = foundOne.transform.parent.gameObject;
                objectMain.GetComponent<NPCspawn>().PlayerDeath();
            }
            
            foreach (GameObject found in NPCs)
            {
                GameObject objectMain1 = found.transform.parent.gameObject;
                objectMain1.GetComponent<NPCMove>().Die();
            }
            

        }

    }
    
    public void Healfull()
    {
        hp = maxhp;
        healthSlider.value = hp;
        Mp = MaxMp;
        MpSlider.value = Mp;
        
        

    }

    public void GetExp(float GetEXP)
    {
        EXP = EXP + GetEXP;
        exptext.text = "Exp : " + EXP;

    }

    public void LoseEXP(float EXPLost)
    {
        EXP = EXP- EXPLost;
    }
    
    public void IncreaseHealth(int HealthIncreaseAmount)
    {
        if (EXPCost <= EXP)
        {
            maxhp = maxhp + HealthIncreaseAmount;
            healthSlider.maxValue = maxhp;
            EXP = EXP - EXPCost;
            exptext.text = "Exp : " + EXP;
            EXPCost = EXPCost + (EXPCost * 0.05f) + 15.0f;
            EXPCost = (Mathf.Round(EXPCost * 100)) / 100.0f;
            expcosttext.text = "Upgrade cost : " + EXPCost;
        }
    }

    public void IncreaseMP(int MpIncreaseAmount)
    {
        if (EXPCost <= EXP)
        {
            MaxMp = MaxMp + MpIncreaseAmount;
            MpSlider.maxValue = MaxMp;
            EXP = EXP - EXPCost;
            exptext.text = "Exp : " + EXP;
            EXPCost = EXPCost + (EXPCost * 0.05f) + 15.0f;
            EXPCost = (Mathf.Round(EXPCost * 100)) / 100.0f;
            expcosttext.text = "Upgrade cost : " + EXPCost;
        }
    }

    public void IncreaseMeleeDmg(float MeleeIncreaseAmount)
    {
        if (EXPCost <= EXP)
        {
            MeleeDamage = MeleeDamage + MeleeIncreaseAmount;

            EXP = EXP - EXPCost;
            exptext.text = "Exp : " + EXP;
            EXPCost = EXPCost + (EXPCost * 0.05f) + 15.0f;
            EXPCost = (Mathf.Round(EXPCost * 100)) / 100.0f;
            expcosttext.text = "Upgrade cost : " + EXPCost;
        }
    }
    public void IncreaseMagicDmg(float MagicIncreaseAmount)
    {
        if (EXPCost <= EXP)
        {
            MagicDamage = MagicDamage + MagicIncreaseAmount;

            EXP = EXP - EXPCost;
            exptext.text = "Exp : " + EXP;
            EXPCost = EXPCost + (EXPCost * 0.05f) + 15.0f;
            EXPCost = (Mathf.Round(EXPCost * 100)) / 100.0f;
            expcosttext.text = "Upgrade cost : " + EXPCost;
        }
    }

    public void ElectricAtk()
    {
        if (Mp >= 25)
        {
            StartCoroutine(Electric1());
            Mp = Mp - 25;
            MpSlider.value = Mp;
          
        }
    }
      
    public void IceAtk()
    {
        if(Mp>=25)
        { 
        StartCoroutine(Ice1());
        Mp = Mp - 25;}
        MpSlider.value = Mp;
      
    }
    public void FireAtk()
    {
        if (Mp >= 25)
        {
            StartCoroutine(Fire1());
            Mp = Mp - 25;
            MpSlider.value = Mp;
            
        }
    }
    
    IEnumerator Electric1()
    {
        if (turn == true && moving == false)
        {
            LightningHbox.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            LightningHbox.SetActive(false);
            TurnManager.EndTurn();
        }
        
    }
    IEnumerator Ice1()
    {
        if (turn == true && moving == false)
        {
            IceHbox.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            IceHbox.SetActive(false);
            TurnManager.EndTurn();
        }

    }
    IEnumerator Fire1()
    {
        if (turn == true && moving == false)
        {
            FireHbox.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            FireHbox.SetActive(false);
            TurnManager.EndTurn();
        }

    }
    
    

}

