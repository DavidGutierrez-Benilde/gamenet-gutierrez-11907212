using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPoints : MonoBehaviour
{
    public static RandomPoints instance;

    public List<Transform> spawnPoints = new List<Transform>();


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }   
        else
        {
            instance = this; 
        }
    }

    public int getSpawnPointsAmount()
    {
        return spawnPoints.Count;
    }
}
