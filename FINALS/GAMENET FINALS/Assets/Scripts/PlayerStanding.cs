using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PlayerStanding : MonoBehaviourPunCallbacks
{
    private const byte PLAYER_COUNTER = 0;
    private int playerOrder = PhotonNetwork.PlayerList.Length;

    public void UpdateScores()
    {
        string nickName = photonView.Owner.NickName;
        int viewID = photonView.ViewID;

        object data = new object[] { nickName, playerOrder, viewID };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
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
            playerOrder = (int)data[1];
            int viewID = (int)data[2];



        }
    }
}
