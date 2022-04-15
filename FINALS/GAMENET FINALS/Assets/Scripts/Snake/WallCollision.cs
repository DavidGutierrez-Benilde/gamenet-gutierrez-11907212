using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallCollision : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision Detected");
            SnakeMovement snake = collision.gameObject.GetComponentInParent<SnakeMovement>();

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
