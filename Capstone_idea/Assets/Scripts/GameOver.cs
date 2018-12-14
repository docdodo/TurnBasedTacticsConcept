using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// whenever a player dies they come here and are allowed to restart or quit the game.
public class GameOver : MonoBehaviour
{
    public void StruggleOn()
        {
        SceneManager.LoadScene(0);
        }

    public void Surrender()
    {
        Application.Quit();
    }
}
