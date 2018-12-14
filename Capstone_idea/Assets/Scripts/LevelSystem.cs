using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour {

    
    public GameObject player;
    public float AtkUpreduced;
    public float MagicAtkUpreduced;
    public void IncreaseHealth()
    {
        player.GetComponent<PlayerMove>().IncreaseHealth(10);
    }
    public void IncreaseMP()
    {
        player.GetComponent<PlayerMove>().IncreaseMP(10);
    }
    public void IncreaseMeleeAtk()
    {
        AtkUpreduced = 40.0f / player.GetComponent<PlayerMove>().MeleeDamage;
        player.GetComponent<PlayerMove>().IncreaseMeleeDmg(AtkUpreduced);
    }
    public void IncreaseMagicAtk()
    {
        MagicAtkUpreduced = 40.0f / player.GetComponent<PlayerMove>().MagicDamage;
        player.GetComponent<PlayerMove>().IncreaseMagicDmg(MagicAtkUpreduced);
    }

}
