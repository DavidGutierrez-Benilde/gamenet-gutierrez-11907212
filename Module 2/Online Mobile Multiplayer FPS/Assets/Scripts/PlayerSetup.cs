using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Photon.Pun; 

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject fpsModel;
    public GameObject nonFpsModel;

    public GameObject playerUIPrefab; 

    public PlayerMovementController playerMovementController;
    public Camera fpsCamera;

    private Animator animator;
    public Avatar fpsAvatar, nonFpsAvatar; 

    void Start()
    {
        playerMovementController = this.GetComponent<PlayerMovementController>();
        animator = this.GetComponent<Animator>();

        fpsModel.SetActive(photonView.IsMine);
        nonFpsModel.SetActive(!photonView.IsMine);

        animator.SetBool("isLocalPlayer", photonView.IsMine);
        animator.avatar = photonView.IsMine ? fpsAvatar : nonFpsAvatar;

        if (photonView.IsMine)
        {
            GameObject playerUI = Instantiate(playerUIPrefab);
            playerMovementController.fixedTouchField = playerUI.transform.Find("RotationTouchField").
                                                                                          GetComponent<FixedTouchField>();
            playerMovementController.joystick = playerUI.transform.Find("Fixed Joystick").GetComponent<Joystick>();
            fpsCamera.enabled = true; 
        }
        else
        {
            playerMovementController.enabled = false;
            GetComponent<RigidbodyFirstPersonController>().enabled = false;
            fpsCamera.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
