using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float fireDamage = 25f;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !collider.gameObject.GetComponent<PhotonView>().IsMine)
        {
            collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, fireDamage);
        }

        this.gameObject.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
