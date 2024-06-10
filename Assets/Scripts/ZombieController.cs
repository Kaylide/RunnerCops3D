using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject PlayerSpawnerGO;
    public ZombieSpawnerController zombieSpawner;
    bool isZombieAlive;
    // Start is called before the first frame update
    void Start()
    {
        isZombieAlive = true;
    }

    private void FixedUpdate()
    {
        if (zombieSpawner.isZombieAttack == true) 
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerSpawnerGO.transform.position, Time.fixedDeltaTime * 1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && isZombieAlive == true)
        {
            isZombieAlive = false;
            zombieSpawner.ZombieAttackThisCop(collision.gameObject, gameObject); 
            Destroy(gameObject);
            // if the zombie collides with the player, set isZombieAlive to false, call the ZombieAttackThisCop method from the ZombieSpawnerController script, 
            // destroy the zombie GameObject
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            zombieSpawner.ZombieGotShoot(gameObject); // Call the ZombieGotShoot method from the ZombieSpawnerController script
            Destroy(other.gameObject); // Destroy the bullet GameObject
        }
    }
}
