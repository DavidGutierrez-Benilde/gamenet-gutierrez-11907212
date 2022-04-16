using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Apple : MonoBehaviourPunCallbacks
{
    AppleSpawn appleSpawn;
    // Start is called before the first frame update
    void Start()
    {
        appleSpawn = GameObject.FindObjectOfType<AppleSpawn>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SnakeMovement snake = other.gameObject.GetComponentInParent<SnakeMovement>();
            snake.gameObject.GetComponent<PhotonView>().RPC("AddBodyPart", RpcTarget.AllBuffered);
            this.gameObject.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
            appleSpawn.SpawnFood();
        }
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
