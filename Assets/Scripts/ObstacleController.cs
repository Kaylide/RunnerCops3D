using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private PlayerSpawnerController playerSpawner;
    private GameObject playerSpawnerGO;
    // Start is called before the first frame update
    void Start()
    {
        playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner"); // Find the GameObject with the tag "PlayerSpawner"
        playerSpawner = playerSpawnerGO.GetComponent<PlayerSpawnerController>(); // Get the PlayerSpawnerController component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerSpawner.CopsGotEliminated(other.gameObject); // Call the CopsGotEliminated method from the PlayerSpawnerController script
        }
    }
}
