using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MenuScript : MonoBehaviour
{
    [MenuItem("Tools/Assign Tile Material")]
    public static void AssigntileMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");
        Material material = Resources.Load<Material>("tile");
        foreach (GameObject t in tiles)
        {
            t.GetComponent<Renderer>().material = material;
        }
    }

    [MenuItem("Tools/Assign Tile script")]
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");
       
        foreach (GameObject t in tiles)
        {
            t.AddComponent<tile>();
        }
    }
}

