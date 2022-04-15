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
            SnakeMovement snake = collision.gameObject.GetComponentInParent<SnakeMovement>();

            snake.GetComponent<PhotonView>().RPC("RemoveBodyPart", RpcTarget.AllBuffered);
        }
    }
}
