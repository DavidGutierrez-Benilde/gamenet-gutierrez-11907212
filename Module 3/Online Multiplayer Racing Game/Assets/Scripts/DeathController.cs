using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class DeathController : MonoBehaviourPunCallbacks
{
    private int eliminationOrder = PhotonNetwork.PlayerList.Length;

    private const byte WHO_KILLED_FIRST = 0;

    public void GameFinished()
    {
        GetComponent<PlayerSetup>().camera.transform.parent = null;
        GetComponent<VehicleMovement>().enabled = false;
        GetComponent<Shooting>().enabled = false;

        eliminationOrder--;

        string nickName = photonView.Owner.NickName;
        int viewID = photonView.ViewID;


        object data = new object[] { nickName, eliminationOrder, viewID };


        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };


        PhotonNetwork.RaiseEvent(WHO_KILLED_FIRST, data, raiseEventOptions, sendOption);
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
        if (photonEvent.Code == WHO_KILLED_FIRST)  // Checks if event code is from WhoKilledFirst
        {
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfKilledPlayer = (string)data[0];
            eliminationOrder = (int)data[1];
            int viewID = (int)data[2];

            Debug.Log(nickNameOfKilledPlayer + " " + eliminationOrder);
            GameObject eliminationUIText = RacingGameManager.instance.finisherTextUI[eliminationOrder + 1];
            eliminationUIText.SetActive(true);

            if (viewID == photonView.ViewID) // this is you
            {
                eliminationUIText.GetComponent<Text>().text = (eliminationOrder + 1) + " " + nickNameOfKilledPlayer + "(YOU)";
                eliminationUIText.GetComponent<Text>().color = Color.red;
            }
            else
            {
                eliminationUIText.GetComponent<Text>().text = (eliminationOrder + 1) + " " + nickNameOfKilledPlayer;
            }

            if (eliminationOrder < 1)
            {
                Text winnerText = RacingGameManager.instance.timeText;
                winnerText.GetComponent<Text>().text = nickNameOfKilledPlayer + " " + "WINS!";
            }
        }
    }

    public int GetEliminationOrder()
    {
        return eliminationOrder;
    }
}
