using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class Bullet : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 
                                                            other.gameObject.GetComponent<Shooting>().fireDamage);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
