using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab; 
    
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            /*
            int randomPointX = Random.Range(-10, 10);
            int randomPointZ = Random.Range(-10, 10);
            */

            Vector3 spawnLocation = RandomPoints.instance.spawnPoints
                                        [Random.Range(0, RandomPoints.instance.spawnPoints.Count)].transform.position;
            PhotonNetwork.Instantiate(playerPrefab.name, spawnLocation, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players;
        GameObject winner; 

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in players)
        {
            if(p.GetComponent<Shooting>().killCount >= 10)
            {
                Debug.Log(p.GetComponent<PhotonView>().Owner.NickName + " wins.");
                winner = p; 

                foreach(GameObject pl in players)
                {
                     pl.GetComponent<PhotonView>().RPC("UpdateKillLog", RpcTarget.All,
                        winner.GetComponent<PhotonView>().Owner.NickName + " wins.");
                }
            }
        }
    }
}
