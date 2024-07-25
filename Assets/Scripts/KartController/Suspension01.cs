using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension01 : MonoBehaviour
{
    public Transform wheel;
    private Rigidbody body;
    private KartController01 kart;
    public bool driftMode;

    float forwardInput;

    //private ConfigurableJoint joint;    

    [Header("Suspension")] //set these with scriptable object?
    private float maxLength, minLength, springForce, lastLength, dampForce, springVel;
    public float restLength, springTravel, springStiffness, dampStiffness, springLength;

    public RaycastHit hit;

    [Header("Wheel")]
    public float wheelRadius, steerAngle, steerTime;   //can get this via code, no?
    public float driftModeValue;
    Vector3 prevWheelPos;
    Vector3 refVelocity;
    public bool wheelFrontLeft, wheelFrontRight, wheelRearLeft, wheelRearRight;         //drop down/enum would be way better
    float slipAngle, slipFactor;
    float enginePower;

    private Vector3 suspensionForce, wheelVelocityLS, wheelPos;    //is this needed? Declare in seperate function

    private float wheelAngle, Fx, Fy;


    void Start()    //set required components
    {
        body = GetComponentInParent<Rigidbody>();
        kart = GetComponentInParent<KartController01>();

        wheel = Instantiate(kart.wheelPrefab, this.transform).transform;
        
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
        lastLength = minLength;

        refVelocity = Vector3.zero;
        driftModeValue = 50.0f;
    }

    void Update()
    {
        if (driftMode)
        {
            driftModeValue = 500f;
        }
        else
        {
            driftModeValue = 5000f;
        }
        //forwardInput = Input.GetAxis("Vertical");
        /*
        if (forwardInput == 0 && Input.GetButton("Fire1"))
        {
            forwardInput = 1.0f;
        }
        */
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
        slipAngle = Vector3.Dot(-wheel.right, body.velocity.normalized);
        //slipAngle = 
        slipFactor = kart.slipCurve.Evaluate(slipAngle);
        slipFactor *= 25.0f * body.velocity.magnitude;
        slipFactor = Mathf.Clamp(slipFactor, 10, driftModeValue);
        Debug.DrawLine(wheel.position, wheel.position + wheel.forward);
        Debug.DrawLine(kart.transform.position, kart.transform.position + body.velocity);
        //Debug.Log(slipAngle);

        Vector3 wheelCentrePos = hit.point + Vector3.up;
        wheel.position = Vector3.SmoothDamp(wheelPos, wheelCentrePos, ref refVelocity, 0.3f);
        prevWheelPos = wheelCentrePos;
        

        Debug.DrawRay(transform.position, -transform.up * springLength, Color.green);
        Debug.DrawRay(transform.position + transform.forward, -transform.up * (springLength + wheelRadius), Color.red);
        Debug.DrawRay(transform.position - transform.forward, -transform.up * (springLength + wheelRadius), Color.red);

        
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance;    //- wheelRadius
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVel = (lastLength - springLength) / Time.deltaTime;
            springForce = springStiffness * (restLength - springLength);
            dampForce = dampStiffness * springVel;
            suspensionForce = (springForce + dampForce) * transform.up;

            wheelVelocityLS = transform.InverseTransformDirection(body.GetPointVelocity(hit.point));
            Fx = (kart.AccelerationInput * kart.enginePower) + Mathf.Clamp(body.velocity.magnitude, 0, 35);
            Fy = wheelVelocityLS.x * (1 + slipFactor) + body.velocity.magnitude;

            body.AddForceAtPosition(suspensionForce + (Fx * transform.forward) + (Fy * -transform.right) * Time.deltaTime, hit.point);

            wheelPos = hit.point + (wheelRadius * transform.up);
        }
        IsGrounded();
    }

    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position + transform.forward, -transform.up, out RaycastHit hitL, maxLength + wheelRadius) && Physics.Raycast(transform.position + transform.forward, -transform.up, out RaycastHit hitR, maxLength + wheelRadius))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(wheelPos - (transform.up * wheelRadius), wheelRadius);
        if (IsGrounded())
        {
            Gizmos.DrawSphere(wheelPos, wheelRadius);
        }
    }
}