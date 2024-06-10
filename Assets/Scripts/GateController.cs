using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum GateType {multiplyType, additionType };
public class GateController : MonoBehaviour
{
    public int gateValue; // Gate value
    public TMPro.TextMeshProUGUI gateText; // Gate text
    public GateType gateType; // Gate type
    bool hasGateUsed; // Check if the gate has been used
    private PlayerSpawnerController playerSpawnerScript; 
    private GameObject playerSpawnerGO;
    private GateHolderController gateHolderScript;

    //Awake is called when the script instance is being loaded
    private void Awake() 
    {
        playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner"); // Find the GameObject with the tag "PlayerSpawner"
        playerSpawnerScript = playerSpawnerGO.GetComponent<PlayerSpawnerController>(); // Get the PlayerSpawnerController script from the playerSpawnerGO
        gateHolderScript = transform.parent.gameObject.GetComponent<GateHolderController>(); // Get the GateHolderController script from the parent GameObject
    }

    // Start is called before the first frame update
    void Start()
    {
        AddGateValueAndSymbol(); // Call the AddGateValueAndSymbol method
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && hasGateUsed == false)
        {
            hasGateUsed = true; // Set hasGateUsed to true
            Debug.Log("Player touched the gate!!");
            playerSpawnerScript.SpawnPlayer(gateValue, gateType); // Call the SpawnPlayer method from the playerSpawnerScript
            gateHolderScript.CloseGate(); // Call the CloseGate method from the gateHolderScript
            Destroy(gameObject); // Destroy the gate
        }
    }

    private void AddGateValueAndSymbol() 
    {
        switch (gateType) 
        {
            case GateType.multiplyType: // If the gate type is multiplyType
                gateText.text = "x" + gateValue.ToString(); // Update the gate text
                break;
            case GateType.additionType: // If the gate type is additionType
                gateText.text = "+" + gateValue.ToString(); // Update the gate text
                break;
            default
                : break;
        }
    }
}
