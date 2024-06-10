using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    public GameObject bulletGO;
    public Transform bulletSpawnTransform;
    public float bulletSpeed = 13f;
    bool isShootingOn = false;
    private Transform playerSpawnerCenter;
    float goToCenterSpeed = 4f;

    public AudioSource playerAudioSource;
    public AudioClip shootingClip;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpawnerCenter = transform.parent.gameObject.transform;
        PlayerSpawnerController playerSpawner = transform.parent.gameObject.GetComponent<PlayerSpawnerController>();
        playerAudioSource = playerSpawner.playerSpawnerAudioSource;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerSpawnerCenter.position, Time.fixedDeltaTime * goToCenterSpeed); 
        // Move player to the center of the player spawner
    }

    public void StartShooting() 
    {
        StartShootingAnim(); // Call the StartShootingAnim method
        isShootingOn = true; // Set isShootingOn to true
        StartCoroutine(Shooting()); // Start the Shooting coroutine
    }

    public void StopShooting() 
    {
        isShootingOn = false; // Set isShootingOn to false
        StartRunningAnim(); // Call the StartRunningAnim method
    }

    private void StartShootingAnim() 
    {
        playerAnimator.SetBool("isShooting", true); // Set the isShooting parameter to true
        playerAnimator.SetBool("isRunning", false); // Set the isRunning parameter to false
    }

    private void StartRunningAnim() 
    {
        playerAnimator.SetBool("isShooting", false); // Set the isShooting parameter to false
        playerAnimator.SetBool("isRunning", true); // Set the isRunning parameter to true      
    }

    public void StartIdleAnim() 
    {
        playerAnimator.SetBool("isRunning", false); // Set the isRunning parameter to false
        playerAnimator.SetBool("isLevelFinished", true); // Set the isLevelFinished parameter to true
    }
    IEnumerator Shooting() 
    {
        while (isShootingOn) 
        {
            yield return new WaitForSeconds(0.5f); // Wait for 0.5 sec
            Shoot(); // Call the Shoot method
            yield return new WaitForSeconds(2f); // Wait for 2 sec 
        }
    }

    private void Shoot() 
    {
        PlayAudio(); // Call the PlayAudio method
        GameObject bullet = Instantiate(bulletGO, bulletSpawnTransform.position, Quaternion.identity); // Instantiate the bullet GameObject
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = transform.forward * bulletSpeed;
    }

    private void PlayAudio() 
    {
        if (playerAudioSource != null) {
            playerAudioSource.PlayOneShot(shootingClip, 0.3f);
        }
    }
}
