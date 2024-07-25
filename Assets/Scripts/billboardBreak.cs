using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class billboardBreak : MonoBehaviour
{
    public float force = 10.0f;

    private Rigidbody[] rigidBodies;

    private void Start()
    {
        // Get all the rigid bodies in the billboard
        rigidBodies = GetComponentsInChildren<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided with the billboard is a car
        if (collision.gameObject.CompareTag("Untagged"))
        {
            // Apply force to each rigid body in the billboard
            foreach (Rigidbody rb in rigidBodies)
            {
                rb.AddForce(collision.impulse / Time.fixedDeltaTime * force, ForceMode.Impulse);
            }
        }
    }
}

