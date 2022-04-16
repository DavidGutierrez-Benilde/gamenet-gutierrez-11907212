using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AppleSpawn : MonoBehaviour
{
    public GameObject applePrefab;
    public Vector3 spawnArea;
    public Vector3 size;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            SpawnFood();
        }
        else
        {
            this.GetComponent<AppleSpawn>().enabled = false;
        }
    }

    private void Update()
    {
        if (GameObject.FindObjectOfType<Apple>() == null)
        {
            SpawnFood();
        }
    }

    // Update is called once per frame
    public void SpawnFood()
    {
        if (GameObject.FindObjectOfType<Apple>() != null) return;

        Vector3 position = spawnArea + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2),
                                                        Random.Range(-size.z / 2, size.z / 2));
        GameObject apple = PhotonNetwork.Instantiate(applePrefab.name, position, Quaternion.identity);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(spawnArea, size);
    }
}
