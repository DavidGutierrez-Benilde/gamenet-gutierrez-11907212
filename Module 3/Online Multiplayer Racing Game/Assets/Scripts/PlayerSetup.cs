using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        this.camera = transform.Find("Camera").GetComponent<Camera>();
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        {
            GetComponent<VehicleMovement>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = photonView.IsMine;
            GetComponent<DeathController>().enabled = false;
            GetComponent<Shooting>().enabled = false;
            camera.enabled = photonView.IsMine;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        {
            GetComponent<VehicleMovement>().enabled = photonView.IsMine;
            GetComponent<DeathController>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = false;
            GetComponent<Shooting>().enabled = photonView.IsMine;
            camera.enabled = photonView.IsMine;
        }
    }
}
