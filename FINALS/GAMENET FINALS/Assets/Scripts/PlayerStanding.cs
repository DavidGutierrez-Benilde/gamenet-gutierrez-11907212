using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using TMPro;

public class PlayerStanding : MonoBehaviourPunCallbacks
{
    private const byte PLAYER_COUNTER = 0;

    private void Start()
    {
        UpdateScores(this.GetComponent<SnakeMovement>().GetAmountEaten());
    }

    public void UpdateScores(int value)
    {
        string nickName = photonView.Owner.NickName;
        int playerNumber = photonView.Owner.ActorNumber;
        int score = value;
        int viewID = photonView.ViewID;

        object data = new object[] { nickName, playerNumber, score, viewID };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.DoNotCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };


        PhotonNetwork.RaiseEvent(PLAYER_COUNTER, data, raiseEventOptions, sendOption);
    }

    private void OnEnable() // a method that is called when we receive an event, a listener when an event is called by photon
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnDisable() // when a listener for an event is removed
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    void OnEventReceived(EventData photonEvent)
    {
        if (photonEvent.Code == PLAYER_COUNTER)
        {
            object[] data = (object[])photonEvent.CustomData;
            string playerNickName = (string)data[0];
            int playerNumber = (int)data[1];
            int score = (int)data[2];
            int viewID = (int)data[3];

            Debug.Log("Event Raised");
            Debug.Log("Score: " + score);
            Debug.Log("PlayerNo: " + playerNumber);

            GameObject scoresText = UIManager.instance.playerStandings[playerNumber - 1];


            if (viewID == photonView.ViewID)
                scoresText.GetComponent<TextMeshProUGUI>().text = playerNickName + " (YOU) " + score;
            else
                scoresText.GetComponent<TextMeshProUGUI>().text = playerNickName + " " + score;
        }
    }
}
