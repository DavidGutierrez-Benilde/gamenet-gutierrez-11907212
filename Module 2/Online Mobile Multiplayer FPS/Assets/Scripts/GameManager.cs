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
            int randomPointX = Random.Range(-10, 10);
            int randomPointZ = Random.Range(-10, 10);
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPointX, 0, randomPointZ), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players;

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in players)
        {
            if(p.GetComponent<Shooting>().killCount >= 10)
            {
                Debug.Log(p.GetComponent<PhotonView>().Owner.NickName + " wins.");
            }
        }
    }
}
