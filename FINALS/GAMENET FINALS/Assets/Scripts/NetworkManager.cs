using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [Header("Login UI Panel")]
    public GameObject loginUIPanel;
    public TMP_InputField usernameInput;
    public TextMeshProUGUI buttonText;


    #region UI callbacks
    public void OnConnectButtonClicked()
    {
        string playerName = usernameInput.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player Name is Invalid");
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            buttonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion
    #region Unity Functions
    #endregion
    #region PUN Callbacks

    public override void OnConnectedToMaster()
    {

    }
    #endregion
}
