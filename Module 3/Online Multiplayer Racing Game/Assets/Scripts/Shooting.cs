using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public bool isControlEnabled;

    [Header("HP Settings")]
    public float startHealth = 100;
    [SerializeField] private float health;

    [Header("Gun Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float fireDamage = 1f;
    private float nextFire = 0f;

    [Header("Laser Settings")]
    public bool isLaser;

    [Header("Is Projectile Settings")]
    public bool isProjectile;
    public Transform firePoint;

    void Awake()
    {
        if (isLaser)
        {
            firePoint = null;
        }

        if (isProjectile)
        {
            bulletPrefab.GetComponent<Bullet>().fireDamage = fireDamage;
        }
    }

    void Start()
    {
        this.GetComponent<PhotonView>().RPC("SetHealth", RpcTarget.AllBuffered, startHealth);
        isControlEnabled = false;
    }

    void Update()
    {
        if (isControlEnabled)
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                FireWeapon();
            }
            else if (isLaser && Input.GetKeyUp(KeyCode.Space))
            {
                this.gameObject.GetComponent<PhotonView>().RPC("IsLaserOn", RpcTarget.AllBuffered, false);
            }
        }

        if (this.GetComponent<DeathController>().GetEliminationOrder() <= 1)
        {
            Die();
        }
    }

    public void FireWeapon()
    {
        if (isLaser)
        {
            RaycastHit hit;
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            this.gameObject.GetComponent<PhotonView>().RPC("IsLaserOn", RpcTarget.AllBuffered, true);

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
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * (gameObject.GetComponent<VehicleMovement>().speed * 1.75f),
                                                                    ForceMode.Impulse);
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        // this.healthBar.fillAmount = health / startHealth; 

        if (health <= 0) // If player (you) is dead or if you win 
        {
            Die();
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
        }
    }

    [PunRPC]
    public void SetHealth(float value)
    {
        health = value;
    }

    [PunRPC]
    public void IsLaserOn(bool value)
    {
        bulletPrefab.SetActive(value);
    }

    public void Die()
    {
        this.GetComponent<DeathController>().GameFinished();
    }
}
