using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera camera;

    void Start()
    {
        this.GetComponent<SnakeMovement>().enabled = photonView.IsMine;
        camera.enabled = photonView.IsMine;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
