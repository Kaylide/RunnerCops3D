using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform target; // Target to follow
    public Vector3 offset; // Offset from the target
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate() // LateUpdate is called after all Update functions have been called
    {
        if (target != null) {
            transform.position = target.position + offset; // Set the camera position to the target position plus the offset
        }
    }
}
