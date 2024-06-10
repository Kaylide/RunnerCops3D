using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{
    public GameObject playerGO; // Player GameObject
    public List<GameObject> playersList = new List<GameObject>(); // List of players

    float playerSpeed = 5; // Player speed
    float xSpeed; // Player speed in x axis or moving left and right
    float minXPosition = -4.10f; // Minimum x position of the player
    float maxXPosition = 4.10f; // Maximum x position of the player
    bool isPlayerMoving; // Check if the player is moving

    public AudioSource playerSpawnerAudioSource; // PlayerSpawner audio source
    public AudioClip gateClip, congratsClip, failClip; // Audio clips
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMoving == false) 
        {
            return;
        } // If the player is not moving, return 

        float touchX = 0; // Touch position in x axis
        float newXValue = 0; // New x position of the player
        
        //On mobile devices
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) // check if the first touch phase is moved
        {
            xSpeed = 250; // Set xSpeed to 250
            touchX = Input.GetTouch(0).deltaPosition.x / Screen.width; // Get the touch position in x axis
        }

        //On Editor
        if (Input.GetMouseButton(0)) // Check if the left mouse button is clicked
        {
            xSpeed = 250; // Set xSpeed to 250
            touchX = Input.GetAxis("Mouse X"); // Get the mouse position in x axis
            //Change the mouseX sensitivity in Edit -> Project Settings -> Input Manager -> Mouse X
        }

        newXValue = transform.position.x + touchX * xSpeed * Time.deltaTime; // Calculate the new x position of the player
        newXValue = Mathf.Clamp(newXValue, minXPosition, maxXPosition); // Limit the player movement in x axis between minXPosition and maxXPosition
        Vector3 playerNewPostition = new Vector3(newXValue, transform.position.y, transform.position.z + Time.deltaTime * playerSpeed); // Move player forward in z axis
        transform.position = playerNewPostition; // Update player position
    }

    public void SpawnPlayer(int gateValue, GateType gateType)  // Spawn player method
    {
        PlayAudio(gateClip); // Play the gateClip audio
        //float newYPosition = 0f; // New y position of the player
        // Added because the player spawned is not touching the ground


        if (gateType == GateType.additionType) 
        {
            // Show the debug message that how many cops will be spawned
            Debug.Log("Spawning " + gateValue + " new cops");
            for (int i = 0; i < gateValue; i++)  // Loop through the gateValue
            {
                Debug.Log("Spawning new cop");
                Vector3 playerPosition = GetPlayerPosition();
                GameObject newPlayerGO = Instantiate(playerGO, playerPosition, Quaternion.identity, transform);
                // Instantiate the player GameObject at a random position within a sphere of radius 0.1 
                // Quaternion.identity: This parameter represents the rotation of the instantiated object. In this case, we're using Quaternion.identity, which means no rotation.
                // transform: This parameter represents the parent transform of the instantiated object.In this case, we're setting it to transform, which refers to the transform of the PlayerSpawnerController object itself.
                playersList.Add(newPlayerGO); // Add the new player to the playersList
            } // If the gateType is additionType, spawn new players based on the gateValue
        } else if (gateType == GateType.multiplyType) 
        {
            int newPlayerCount = (playersList.Count * gateValue) - playersList.Count;
            // Show the debug message that how many cops will be spawned
            Debug.Log("Spawning " + newPlayerCount + " new cops");
            for (int i = 0; i < newPlayerCount; i++) 
            {
                Debug.Log("Spawning new cop");
                Vector3 playerPosition = GetPlayerPosition();
                GameObject newPlayerGO = Instantiate(playerGO, playerPosition, Quaternion.identity, transform);
                playersList.Add(newPlayerGO);
            } // If the gateType is multiplyType, spawn new players based on the gateValue
        }

    }

    public Vector3 GetPlayerPosition() 
    {
        Vector3 postition = Random.insideUnitSphere * 0.1f; // Get a random position within a sphere of radius 0.1
        Vector3 newPos = transform.position + postition; // Calculate the new position of the player
        return newPos; // Return the new position of the player
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine") 
        {
            isPlayerMoving = false; // If the player collides with the FinishLine, set isPlayerMoving to false
            ChangeAllCopsAnimToIdleAnim(); // Call the ChangeAllCopsAnimToIdleAnim method to change the animation of all cops to idle animation
            GameManager.instance.ShowCongratsPanel(); // Call the ShowCongratsPanel method from the GameManager when the player reaches the FinishLine
            StopBackgroundMusic(); // Call the StopBackgroundMusic method
            PlayAudio(congratsClip); // Play the congratsClip audio
        } 

    }

    public void CopsGotEliminated(GameObject copGO)
    {
        playersList.Remove(copGO); // Remove the cop GameObject from the playersList
        Destroy(copGO); // Destroy the cop GameObject
        CheckCopsCount(); // Call the CheckCopsCount method to check the number of cops
    }

    private void CheckCopsCount() 
    {
        if (playersList.Count <= 0) 
        {
            Debug.Log("All cops are eliminated");
            // stop PlayerSpawner moving 
            StopPlayer();
            StopBackgroundMusic(); // Call the StopBackgroundMusic method
            PlayAudio(failClip); // Play the failClip audio
            GameManager.instance.ShowFailPanel(); // Call the ShowFailPanel method from the GameManager
        }
    }

    public void EnemyDetected(GameObject target) 
    {
        isPlayerMoving = false; // Set isPlayerMoving to false
        LookAtEnemy(target); // Call the LookAtEnemy method
        StartAllCopsShooting(); // Call the StartAllCopsShooting method
    }

    private void LookAtEnemy(GameObject target) 
    {
        Vector3 dir = target.transform.position - transform.position; // Calculate the direction of the target
        Quaternion lookRot = Quaternion.LookRotation(dir); // Calculate the rotation of the target
        lookRot.x = 0; // Set the x rotation to 0 
        lookRot.z = 0; // Set the z rotation to 0
        transform.rotation = lookRot; // Set the rotation of the player to the rotation of the target
    }

    public void LookAtFoward() 
    {
        transform.rotation = Quaternion.identity; // Set the rotation of the player to the default rotation
    }
    public void AllZombiesKilled() // call this method when zombies count is less than or equal to 0
    {
        LookAtFoward();  // Call the LookAtFoward method to set the player rotation to the default rotation
        MovePlayer(); // Call the MovePlayer method to move the player forward
    }

    public void MovePlayer() 
    {
        isPlayerMoving = true; // Set isPlayerMoving to true
        ChangeAllCopsAnimToRunningAnim(); // Call the ChangeAllCopsAnimToRunningAnim method to change the animation of all cops to running animation
    }

    private void StopPlayer() 
    {
        isPlayerMoving = false; // Set isPlayerMoving to false
    }

    private void StartAllCopsShooting() 
    {
        for (int  i = 0; i < playersList.Count; i++) 
        {
            PlayerController cop = playersList[i].GetComponent<PlayerController>(); // Get the PlayerController component of the cop
            cop.StartShooting(); // Call the StartShooting method
        } 
    }

    private void ChangeAllCopsAnimToRunningAnim() 
    {
        for (int i = 0; i < playersList.Count; i++) 
        {
            PlayerController cop = playersList[i].GetComponent<PlayerController>();
            cop.StopShooting(); // Call the StopShooting method
        }
    }

    private void ChangeAllCopsAnimToIdleAnim()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            PlayerController cop = playersList[i].GetComponent<PlayerController>();
            cop.StartIdleAnim(); // Call the StartIdleAnim method
        }
    }

    private void PlayAudio(AudioClip audio) 
    {
        if (playerSpawnerAudioSource != null)  // Check if the playerSpawnerAudioSource is not null
        {
            playerSpawnerAudioSource.PlayOneShot(audio, 0.5f); // Play the audio clip with a volume of 0.5
        }
    }

    private void StopBackgroundMusic() 
    {
        Camera.main.GetComponent<AudioSource>().Stop(); // Stop the background music
    }
}
