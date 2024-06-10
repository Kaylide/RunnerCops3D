using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerController : MonoBehaviour
{
    public GameObject zombie;
    public List<GameObject> zombies = new List<GameObject>();
    private PlayerSpawnerController playerSpawner;
    private GameObject playerSpawnerGO;
    public bool isZombieAttack;
    public int zombieCount;

    private void Awake()
    {
        playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner"); // Find the GameObject with the tag "PlayerSpawner"
        playerSpawner = playerSpawnerGO.GetComponent<PlayerSpawnerController>(); // Get the PlayerSpawnerController component
        isZombieAttack = false; // Set isZombieAttack to false
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnZombie(zombieCount); // Spawn the zombies
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnZombie(int zombieCount)
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Quaternion zombieRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            GameObject zombieGO = Instantiate(zombie, GetZombiePosition(), zombieRotation, transform);
            ZombieController zombieScript = zombieGO.GetComponent<ZombieController>();
            zombieScript.PlayerSpawnerGO = playerSpawnerGO;
            zombieScript.zombieSpawner = this;
            zombies.Add(zombieGO);
        } 
    }

    public Vector3 GetZombiePosition()
    {
        Vector3 pos = Random.insideUnitSphere * 0.1f;
        Vector3 newPos = transform.position + pos;
        return newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false; // Disable the BoxCollider
            playerSpawner.EnemyDetected(gameObject); // Call the EnemyDetected method from the PlayerSpawnerController script
            LookAtPlayer(other.gameObject); // Call the LookAtPlayer method
            isZombieAttack = true; // Set isZombieAttack to true
        }
    }

    private void LookAtPlayer(GameObject target) 
    {
        Vector3 dir = transform.position - target.transform.position; // Get the direction from the zombie to the player
        Quaternion lookRot = Quaternion.LookRotation(dir); // Get the rotation to look at the player
        lookRot.x = 0; // Set the x rotation to 0
        lookRot.z = 0; // Set the z rotation to 0

        transform.rotation = lookRot; // Rotate the zombie to look at the player
    }

    public void ZombieAttackThisCop(GameObject cop, GameObject zombie)  // if a zombie collides with a cop, call this method and remove the cop and the zombie
    {
        zombies.Remove(zombie); // Remove the zombie from the zombies list
        CheckZombieCount(); // Call the CheckZombieCount method
        playerSpawner.CopsGotEliminated(cop); // Call the CopsGotEliminated method from the PlayerSpawnerController script and remove the cop
    }

    public void ZombieGotShoot(GameObject zombie) 
    {
        zombies.Remove(zombie); // Remove the zombie from the zombies list
        Destroy(zombie); // Destroy the zombie GameObject
        CheckZombieCount(); // Call the CheckZombieCount method
    }

    private void CheckZombieCount() 
    {
        if (zombies.Count <= 0)
        {
            playerSpawner.AllZombiesKilled();
        }
    }
}
