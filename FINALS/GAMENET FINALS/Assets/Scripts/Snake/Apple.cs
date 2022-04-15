using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    AppleSpawn appleSpawn;
    // Start is called before the first frame update
    void Start()
    {
        appleSpawn = GameObject.FindObjectOfType<AppleSpawn>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SnakeMovement snake = other.gameObject.GetComponentInParent<SnakeMovement>();
            snake.AddBodyPart();
            Destroy(this.gameObject);
            appleSpawn.SpawnFood();
        }
    }
}
