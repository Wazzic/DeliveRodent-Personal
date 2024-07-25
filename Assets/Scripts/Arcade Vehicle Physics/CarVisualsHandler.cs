using System;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;

public class CarVisualsHandler : MonoBehaviour
{
    [SerializeField] private SpringToScale scaleSpring;

    [SerializeField] private float suspensionMaxDistance = 2.5f;
    [SerializeField] private float bodyTiltZAmount = 15f;
    [SerializeField] private float driftVisualMultipler = 30f;
    [SerializeField] private AnimationCurve driftCurve;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceYDistance;
    [SerializeField] private bool showDebugRays;
    private ArcadeVehicleController arcadeVehicleController;

    
    Transform carHolder;    
    [SerializeField] Transform carBody;
    
    [SerializeField] private float speedLinesThreshold;

    public Transform[] AllWheelHolders;
    Transform[] frontWheelHolders = new Transform[2];

    public GameObject[] DeliveryItems;
    public ParticleEffectGroup Nitrous;
    public MeshRenderer carBodyMeshRendere;
    public GameObject Driver;

    public bool animeLinesPlaying;

    float rayOffsetY;
    private void Awake()
    {
        arcadeVehicleController = GetComponentInParent<ArcadeVehicleController>();
        if (!arcadeVehicleController) return;
        SetupWheelTransforms();
        /*
        DeliveryVFXHandler vfxHandler = arcadeVehicleController.GetComponent<DeliveryVFXHandler>();
        foreach (GameObject item in DeliveryItems)
        {
            vfxHandler.deliverySprings.Add(item.GetComponentInChildren<DeliverySpring>());
        }
        */
        rayOffsetY = arcadeVehicleController.StartRayOffset.y;
        carHolder = transform;
    }
    private void SetupWheelTransforms()
    {
        frontWheelHolders[0] = AllWheelHolders[0];
        frontWheelHolders[1] = AllWheelHolders[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!arcadeVehicleController) return;
        Suspension();
        TireSpinning();
        DriftBody();
        BounceCarBody();
        TiltCarBody();
    }
    private void Suspension()
    {
        //visualise suspension
        for (int i = 0; i < 4; i++)
        {
            Transform wheelMesh = AllWheelHolders[i].GetChild(0);
            if (arcadeVehicleController.WheelIsGrounded[i])
            {
                wheelMesh.localPosition = new Vector3(0, -arcadeVehicleController.WheelRayHits[i].distance + 1 + rayOffsetY, 0);
                //wheelMesh.position = arcadeVehicleController.WheelRayHits[i].point;
            }
            else
            {
                wheelMesh.localPosition = new Vector3(0, -0.5f, 0);
                //wheelMesh.localPosition = new Vector3(0, -suspensionMaxDistance + 1, 0);
            }
        }
    }

    private void DriftBody()
    {
        //if drifting and grounded
        if (arcadeVehicleController.LockedIntoDrift)
        {
            //sign will be -1 if car is going left, 1 if it is going right
            float sign = Mathf.Sign(-arcadeVehicleController.carRelativeVelocity.x);
            //arcadeVehicleController.carVelocity.x seems to go between 0 and 50 so multiplying it by 0.02 make it between 0 and 1
            float _absoluteTurnVelocity = Mathf.Abs(arcadeVehicleController.carRelativeVelocity.x * -0.02f);

            float _driftAmount = driftCurve.Evaluate(_absoluteTurnVelocity);
            //mutliply it by the drift multiplier and sign so its negative if going left and positive if going right
            float _driftAmountMultiplid =  driftVisualMultipler * (int)sign;
            Vector3 _targetRot;
            if (Mathf.Abs(_driftAmount) > 0.01f)
            {
                _targetRot = new Vector3(carHolder.localRotation.eulerAngles.x, _driftAmountMultiplid, carHolder.localRotation.eulerAngles.z);
            }
            else
            {
                _targetRot = new Vector3(carHolder.localRotation.eulerAngles.x, 0, carHolder.localRotation.eulerAngles.z);
            }
            carHolder.localRotation = Quaternion.Slerp(carHolder.localRotation, Quaternion.Euler(_targetRot), 5f * Time.deltaTime);
        }
        else
        {
            //reset rotation over time
            carHolder.localRotation = Quaternion.Slerp(carHolder.localRotation, Quaternion.Euler(Vector3.zero), 10f * Time.deltaTime);
        }
    }
    [SerializeField] float bodyLerpTime;
    private void TiltCarBody()
    {
        //float _lerpTime;
        Quaternion _targetRot;
        //if steering
        if (Mathf.Abs(arcadeVehicleController.SteerInput) > 0.1)
        {
            if (!arcadeVehicleController.IsGrounded)
                return;
            // Set target rotations
            if (arcadeVehicleController.HandBrakeInput)
            {
                _targetRot = Quaternion.Euler(carBody.localRotation.x, carBody.localRotation.y, arcadeVehicleController.DriftSteerInput * bodyTiltZAmount * arcadeVehicleController.CurrentSpeed);

            }
            else
            {
                _targetRot = Quaternion.Euler(carBody.localRotation.x, carBody.localRotation.y, arcadeVehicleController.SteerInput * bodyTiltZAmount * arcadeVehicleController.CurrentSpeed);
            }
            //_lerpTime = 10f;
        }
        else
        {
            _targetRot = Quaternion.Euler(carBody.localRotation.x, carBody.localRotation.y, 0);
            //_lerpTime = 15f;
        }
        carBody.localRotation = Quaternion.Slerp(carBody.localRotation, _targetRot, bodyLerpTime * Time.deltaTime);

    }
    private void BounceCarBody()
    {
        //bounce
        //
        float _yPos = Mathf.PingPong(Time.time * bounceSpeed, 2) - 1;
        _yPos *= bounceYDistance;
        carBody.localPosition = new Vector3(carBody.localPosition.x, _yPos, carBody.localPosition.z);
    }
    public void BodyScaleSpring(Vector3 nudgeVector)
    {
        scaleSpring.Nudge(nudgeVector);
    }

    private void TireSpinning()
    {
        float m_steerInput = arcadeVehicleController.SteerInput;
        //tires
        for (int i = 0; i < frontWheelHolders.Length; i++)
        {
            //lerp front wheels on the y axis depending on horizontal input
            var FW = frontWheelHolders[i];
            FW.localRotation = Quaternion.Slerp(FW.localRotation, 
                Quaternion.Euler(FW.localRotation.eulerAngles.x, 30 * m_steerInput, FW.localRotation.eulerAngles.z), 0.1f);

            //FW.GetChild(0).localRotation = transform.localRotation;
        }

        //rearWheelHolders[0].GetChild(0).localRotation = transform.localRotation;
        //rearWheelHolders[1].GetChild(0).localRotation = transform.localRotation;
    }
}
