using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour {

    
    public GameObject player;
    public float AtkUpreduced;
    public float MagicAtkUpreduced;
    public float hpupreduced;
    public float mpupreduced;
    public void IncreaseHealth()
    {
        hpupreduced = 200.0f / player.GetComponent<PlayerMove>().hp;
        player.GetComponent<PlayerMove>().IncreaseHealth(hpupreduced);
    }
    public void IncreaseMP()
    {
        mpupreduced = 200.0f / player.GetComponent<PlayerMove>().Mp;
        player.GetComponent<PlayerMove>().IncreaseMP(mpupreduced);
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
