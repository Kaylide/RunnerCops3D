using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHolderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseGate()
    {
        for (int i = 0; i < transform.childCount; i++) // Loop through all the children of the GateHolder
        {
            if (transform.GetChild(i) != null) // Check if the child exists
            {
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = false; // Disable the BoxCollider component
            }
        }
    }
}
