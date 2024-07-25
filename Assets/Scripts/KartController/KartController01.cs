using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class KartController01 : MonoBehaviour
{

    [SerializeField] public GameObject wheelPrefab;    //Wheel to be instantiated onto vehicle

    private GameObject[] wheels = new GameObject[4];    //Array containing the wheel objects
    public Suspension01[] suspension = new Suspension01[4];
    private Transform[] wheelSlots = new Transform[4];  //Array containing to wheels default position

    float[] steeringAngle = new float[4];

    Rigidbody rb;
    [SerializeField] private Transform CoG;

    [Header("Engine")] public float enginePower;

    [Header("Car Specs")]
    public float wheelBase, rearTrack;
    float turnRadius;
    public float minTurnRadius, maxTurnRadius;
    public AnimationCurve slipCurve;
    public AnimationCurve enginePowerCurve;

    [Header("Inputs")]
    public float SteerInput;
    public float AccelerationInput;
    public float BrakeInput;
    public bool HandBrakeInput;

    private float ackermanAngleLeft;
    private float ackermanAngleRight;

    public void OnSteerInput(InputAction.CallbackContext context)
    {
        SteerInput = context.ReadValue<float>();
    }
    public void OnAccelerateButton(InputAction.CallbackContext context)
    {
        AccelerationInput = context.ReadValue<float>();
    }
    public void OnBrakeInput(InputAction.CallbackContext context)
    {
        BrakeInput = context.ReadValue<float>();
    }
    public void OnHandBrakeButton(InputAction.CallbackContext context)
    {
        HandBrakeInput = context.action.WasPressedThisFrame();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CoG.localPosition;

        suspension = GetComponentsInChildren<Suspension01>();       

        for (int i = 0; i < wheelSlots.Length; i++) //Add editor toggle for this
        {
            wheelSlots[i] = transform.GetChild(i);
            
            suspension[i].wheel = wheelSlots[i].transform;
        }
    }

    private void Update()
    {
        
        turnRadius = 0.5f * rb.velocity.magnitude;
        turnRadius = Mathf.Clamp(turnRadius, minTurnRadius, maxTurnRadius);


        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("ShiftPressed");
            suspension[0].driftMode = true;
            suspension[1].driftMode = true;
            suspension[2].driftMode = true;
            suspension[3].driftMode = true;
            SteerInput *= 1.2f;
        }
        else
        {
            suspension[0].driftMode = false;
            suspension[1].driftMode = false;
            suspension[2].driftMode = false;
            suspension[3].driftMode = false;
        }

        if (SteerInput > 0f)  //turning right
        {
            ackermanAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * SteerInput;
            ackermanAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * SteerInput;
        }
        else if (SteerInput < 0f)    //turning left
        {
            ackermanAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * SteerInput;
            ackermanAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * SteerInput;
        }
        else
        {
            ackermanAngleLeft = 0;
            ackermanAngleRight = 0;
        }

        foreach (Suspension01 w in suspension)
        {
            if (w.wheelFrontLeft)
            {
                w.steerAngle = ackermanAngleLeft;
            }
            if (w.wheelFrontRight)
            {
                w.steerAngle = ackermanAngleRight;
            }
        }
        
        //Debug.Log(rb.velocity.magnitude);
    }
}
