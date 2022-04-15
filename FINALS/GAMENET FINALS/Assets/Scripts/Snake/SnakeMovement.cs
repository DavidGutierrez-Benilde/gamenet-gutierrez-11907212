using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SnakeMovement : MonoBehaviourPunCallbacks
{
    public List<Transform> BodyParts = new List<Transform>();

    public float minimalDistance = 0.25f;

    public int initialSize;

    public float speed = 1f;
    public float rotationSpeed = 50f;

    public GameObject bodyPrefab;

    private float distance;
    private Transform currentBodyPart;
    private Transform previousBodyPart;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BodyParts.Count; i++) BodyParts[i].transform.position = SpawnLocations.instance.spawnPoints
                                                        [Random.Range(0, SpawnLocations.instance.spawnPoints.Count)].transform.position;

        if (BodyParts.Count <= 0)
        {
            BodyParts.Add(this.transform.GetChild(0).gameObject.transform);
        }

        for (int i = 0; i < initialSize; i++)
        {
            //AddBodyPart();
            this.GetComponent<PhotonView>().RPC("AddBodyPart", RpcTarget.AllBuffered);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Q))
            this.GetComponent<PhotonView>().RPC("AddBodyPart", RpcTarget.AllBuffered);
    }

    public void Move()
    {
        float currentSpeed = speed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) currentSpeed *= 2f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) currentSpeed /= 2f;

        BodyParts[0].Translate(BodyParts[0].forward * currentSpeed * Time.smoothDeltaTime, Space.World);

        if (Input.GetAxis("Horizontal") != 0)
            BodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));

        this.GetComponent<PhotonView>().RPC("SnakeBodyMovement", RpcTarget.AllBuffered, currentSpeed);
    }

    [PunRPC]
    public void SnakeBodyMovement(float currentSpeed)
    {
        for (int i = 1; i < BodyParts.Count; i++)
        {
            currentBodyPart = BodyParts[i];
            previousBodyPart = BodyParts[i - 1];

            distance = Vector3.Distance(previousBodyPart.position, currentBodyPart.position);


            Vector3 newPosition = previousBodyPart.position;

            newPosition.y = BodyParts[0].position.y;

            float time = Time.deltaTime * distance / minimalDistance * currentSpeed;

            if (time > 0.5f) time = 0.5f;

            currentBodyPart.position = Vector3.Slerp(currentBodyPart.position, newPosition, time);
            currentBodyPart.rotation = Quaternion.Slerp(currentBodyPart.rotation, previousBodyPart.rotation, time);
        }
    }

    [PunRPC]
    public void AddBodyPart()
    {
        Transform newPart = (Instantiate(bodyPrefab, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation)
                                    as GameObject).transform;

        newPart.SetParent(this.transform);
        BodyParts.Add(newPart);
    }

    [PunRPC]
    void RemoveBodyPart()
    {
        for (int i = BodyParts.Count - 1; i > initialSize; i--)
        {
            Destroy(BodyParts[i].gameObject);
            BodyParts.RemoveAt(i);
        }
    }
}
