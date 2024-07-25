using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    [SerializeField] public Suspension01 suspensionR, suspensionL;

    [SerializeField] public float antiRollForce;

    private Rigidbody body;

    [SerializeField] private float antiRoll;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float antiRollForce = (suspensionL.hit.distance - suspensionR.hit.distance) * antiRoll;
        if (suspensionL.IsGrounded())
        {
            body.AddForceAtPosition(suspensionR.transform.up * -antiRollForce, suspensionL.transform.position);
        }
        if (suspensionR.IsGrounded())
        {
            body.AddForceAtPosition(suspensionR.transform.up * antiRollForce, suspensionR.transform.position);
        }
    }
}