using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SnakeCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other) // other == player; 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SnakeMovement snake = other.gameObject.GetComponentInParent<SnakeMovement>();

            for (int i = 0; i < snake.initialSize; i++)
            {
                if (this.gameObject == snake.BodyParts[i].gameObject)
                {
                    Debug.Log("Hit itself");
                    return;
                }
            }

            snake.GetComponent<PhotonView>().RPC("RemoveBodyPart", RpcTarget.AllBuffered);

            Vector3 spawnLocation = SpawnLocations.instance.spawnPoints
                                                   [Random.Range(0, SpawnLocations.instance.spawnPoints.Count)].transform.position;

            while (spawnLocation == snake.transform.position)
                spawnLocation = SpawnLocations.instance.spawnPoints
                                                    [Random.Range(0, SpawnLocations.instance.spawnPoints.Count)].transform.position;

            for (int i = 0; i < snake.BodyParts.Count; i++) snake.BodyParts[i].transform.position = spawnLocation;
        }
    }
}
