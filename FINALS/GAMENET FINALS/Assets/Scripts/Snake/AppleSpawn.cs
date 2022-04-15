using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawn : MonoBehaviour
{
    public GameObject applePrefab;
    public Vector3 spawnArea;
    public Vector3 size;

    void Start()
    {
        SpawnFood();
    }

    // Update is called once per frame
    public void SpawnFood()
    {
        Vector3 position = spawnArea + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2),
                                                        Random.Range(-size.z / 2, size.z / 2));
        Instantiate(applePrefab, position, Quaternion.identity);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(spawnArea, size);
    }
}
