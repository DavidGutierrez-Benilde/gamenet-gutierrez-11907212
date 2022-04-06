using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera; 

    [Header("HP Settings")]
    public float startHealth = 100; 
    private float health; 

    [Header("Gun Settings")]
    public bool isLaser, isProjectile; 
    public float fireRate = 1f; 
    public float fireDamage = 1f; 
    private float nextFire = 0f; 

    [Header("Is Projectile Settings")]
    public GameObject bulletPrefab; 
    public Transform firePoint; 

    void Awake()
    {
        if (isLaser)
        {
            bulletPrefab = null; 
            firePoint = null; 
        }
    }

    void Start()
    {
        health = startHealth; 
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate; 
            FireWeapon(); 
        }
    }

    public void FireWeapon()
    {
        if (isLaser)
        {
            RaycastHit hit;
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out hit, 200))
            {
                Debug.Log(hit.collider.gameObject.name);

                // photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

                if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, fireDamage);

                    /*
                    if(hit.collider.gameObject.GetComponent<Shooting>().health <= 0 &&
                        hit.collider.gameObject.GetComponent<Shooting>().isDead == false)
                    {
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("SetIsDead", RpcTarget.AllBuffered, true);
                    } 
                    */
                }
            }
        }
        else if (isProjectile)
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
            // bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * 2.5f; 
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        // this.healthBar.fillAmount = health / startHealth; 

        if (health <= 0) // If player (you) is dead
        {
            // Die();
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            
            GameObject[] players;

            players = GameObject.FindGameObjectsWithTag("Player");
            
            /*
            foreach (GameObject p in players)
            {
                p.gameObject.GetComponent<PhotonView>().RPC("UpdateKillLog", RpcTarget.All, 
                    info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            }
            */
        }
    }
}
