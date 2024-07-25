using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Lofelt.NiceVibrations;    //Remove this when all haptics have been moved to RumbleController


public struct HitInfo
{
    public bool isFrontWheel;
    public Vector3 wheelHitPoint;
    public Vector3 wheelHitNormal;
}
public class ArcadeVehicleController : MonoBehaviour
{
    
    [SerializeField] private bool ShowDebugRays;

    Transform[] allWheelHolders = new Transform[4];
    [field: SerializeField] public LayerMask DrivableSurfaceMask { get; private set; }

    private PlayerStatus playerStatus;
    private DeliveryVFXHandler vFXHandler;
    private CarVisualsHandler carVisualsHandler;
    //private RumbleController rumbleController;
    [SerializeField] private bool useWheelRotationForDrag;

    [Header("Speed")]
    public float DefaultMaxSpeed;
    public float CurrentMaxSpeed; //will be set to default max speed on start
    [SerializeField] private float MaxZVelocity; //will be set to default max speed on start
    [SerializeField] private float OffRoadMaxSpeed;
    [SerializeField] private float reverseMaxSpeed = 50f; 
    [SerializeField] private float currentAcceleration = 250f;
    [SerializeField] private float defaultAcceleration;
    [SerializeField] private float brakeAcceleration;
    [SerializeField] private float reverseAcceleration;
    [SerializeField] private float forwardTurnMultiplier;
    [SerializeField] private float reverseTurnMultiplier;
    [SerializeField] private float reverseDriftTurnMultiplier;
    [Range(50,500)]
    [SerializeField] private float dragMultiplier = 250f;
    [SerializeField] private float slipStreamMaxSpeed = 100f;

    [Header("Positioning")]
    public Vector3 StartRayOffset;
    [SerializeField] float centerGroundedDistance = 3f;
    [SerializeField] float wheelGroundedDistance = 3f;
    [SerializeField] float YOffset = 1.5f;
    [SerializeField] float maxSteepness = 55f;

    [Header("DownForces")]
    [SerializeField] private float downForceInAirAfterApex = 120f;
    [SerializeField] private float downForceInAirBeforeApex = 100f;

    [Header("Rigidbodies")]
    [SerializeField] public Rigidbody carRigidbody;

    [Header("AirControl")]
    [SerializeField] private float airAcceleration = 5f;
    [SerializeField] private float airMaxSidewaysSpeed = 50f;

    [Header("AnimationCurves")]
    [SerializeField] private AnimationCurve turnCurve;
    [SerializeField] private AnimationCurve driftInputCurve;
    [Tooltip("-0.5 is going down 45 degrees, 0.5 is going up 45 degrees")]
    [SerializeField] private AnimationCurve slopeMultiplierCurve;
    [SerializeField] private AnimationCurve driftTorqueMultiplierCurve;
    [SerializeField] private AnimationCurve driftSidewaysMultiplierCurve;

    [Header("J_Turn")]
    public float jTurnDriftThreshold = 0.5f;
    public float spinForce = 100f;
    [SerializeField] private float jturnspeed;
    //private bool isDriftingReverse = false;

    [Header("Drifting")]
    [SerializeField] private float driftTurnTorque = 25f;
    [SerializeField] private float driftSidewaysForce = 350f;
    [SerializeField] private float[] driftBoostThresholds;
    [SerializeField] private float[] driftBoostSpeeds;
    [SerializeField] private float[] driftBoostAccelerations;
    [SerializeField] private float[] driftBoostDurations;
    //public GamepadRumble driftHaptic;
    

    [Header("SpeedBoosts")]
    [SerializeField] private float boostPanelSpeedIncrease;
    [SerializeField] private float boostPanelAccelerationIncrease;
    [SerializeField] private float boostDefualtAccelerationIncrease;
    [SerializeField] private float boostPanelDuration;
    [SerializeField] private float powerUpBoostSpeedIncrease;
    [SerializeField] private float powerUpBoostAccelerationIncrease;
    [SerializeField] private float powerUpBoostDuration;
    private float initialPowerUpBoostTime;

    bool bumpedWhilstDrifting;
    private float driftTimer;
    private float steeringSign;

    List<Vector3> groundhits = new List<Vector3>();
    public bool[] WheelIsGrounded = new bool[4];
    public RaycastHit[] WheelRayHits = new RaycastHit[4];
    //public Tag[] wheelTerrains = new int[3]; // 0 = default, 1 = grass, 2 = sand
    Vector3[] wheelHitPositions = new Vector3[4];

    bool canHorn = true;
    [SerializeField] float hornDelay = 1.5f;
    
    [HideInInspector]
    public Vector3 carRelativeVelocity;

    [Header("SpeedBoost")]
    private bool isBoosting;
    //public bool IsOffRoading;

    CarAudioHandler carAudioHandler;
    [SerializeField] Transform carBodyHolder;

    public float CurrentSpeed { get; private set; }
    public float SteerInput { get; private set; }
    public float AccelerationInput { get; private set; }
    public float BrakeInput { get; private set; }
    public bool HandBrakeInput { get; private set; }
    public bool LockedIntoDrift { get; private set; }
    public float DriftSteerInput { get; private set; }
    public bool IsGrounded { get; private set; }
    public int CurrentBoostLevel; // { get; private set; }
    public bool IsEmmittingTrail { get; set; }
    public bool IsStunned { get; private set; }
    public int PlayerID { get; private set; }

    public bool isSlipstreamed;

    public float slipStreamTimer;
    private void Start()
    {
        //ID = GetComponent<PlayerLocalManager>().ID;

        //playerInput = GetComponent<PlayerInput>();
        //InputManager.instance.OnNewInput(playerInput);
        vFXHandler = GetComponentInChildren<DeliveryVFXHandler>();
        carAudioHandler = GetComponentInChildren<CarAudioHandler>();
        carVisualsHandler = GetComponentInChildren<CarVisualsHandler>();
        //rumbleController = GetComponent<RumbleController>();

        carBodyHolder = carVisualsHandler.transform;
        allWheelHolders = carVisualsHandler.AllWheelHolders;
        playerStatus = GetComponent<PlayerStatus>();

        //InputManager.instance.LockPlayerCar(carRigidbody);       
    }

    public void OnSteerInput(InputAction.CallbackContext context)
    {
        if (InputManager.instance.controlsEnabled)
        {
            SteerInput = context.ReadValue<float>();
        }
    }
    public void OnAccelerateButton(InputAction.CallbackContext context)
    {
        //if (playerStatus.StunnedForOneFrame || !InputManager.instance.controlsEnabled) 
        if (!InputManager.instance.controlsEnabled) 
        {
            AccelerationInput = 0;
        }
        else
        {
            AccelerationInput = context.ReadValue<float>();
        }       
    }
    public void OnBrakeInput(InputAction.CallbackContext context)
    {
        //if (playerStatus.StunnedForOneFrame || !InputManager.instance.controlsEnabled)
        if (!InputManager.instance.controlsEnabled)
        {
            BrakeInput = 0;
        }
        else
        {
            BrakeInput = context.ReadValue<float>();
        }
    }
    public void OnHandBrakeButton(InputAction.CallbackContext context)
    {
        if (InputManager.instance.controlsEnabled)
        {
            HandBrakeInput = context.action.WasPressedThisFrame();
            carVisualsHandler.BodyScaleSpring(new Vector3(3f, 0.8f, 3f));
        }
    }
    
    public void OnHornButton(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && canHorn)
        {
            canHorn = false;
            carAudioHandler.PlayHornSound();
            StartCoroutine(HornDelay());
        }
    }
    
    RaycastHit centreHit;
    private void Update()
    {
        timeSinceLastCollision += Time.deltaTime;
        
        CurrentSpeed = carRelativeVelocity.z;
        DriftInputHandling();
    }
    
    float driftTorqueMulitplier = 0.5f;
    float driftSidewaysForceMultiplier = 1.5f; // Start at 1.5 of itself
    void FixedUpdate()
    {
        GroundCheck();
        GroundCheckWheels();
        RotateToSurface();
        carRelativeVelocity = carRigidbody.transform.InverseTransformDirection(carRigidbody.velocity);

        // If grounded, set the car's relavite y velocity to 0
        if (IsGrounded)
        {
            carRigidbody.velocity = transform.TransformDirection(new Vector3(carRelativeVelocity.x, 0, carRelativeVelocity.z));
        }
        if (!IsGrounded)
        {
            carRigidbody.drag = 0.2f;
            //carRigidbody.drag = 0f;
            carRigidbody.MoveRotation(Quaternion.Slerp(carRigidbody.rotation, Quaternion.FromToRotation(carRigidbody.transform.up, Vector3.up) * carRigidbody.transform.rotation, 0.05f));
            carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, transform.forward * carRelativeVelocity.z, airAcceleration * Time.deltaTime);
            if (carRigidbody.velocity.y < 5)
            {
                carRigidbody.AddForce(Vector3.down * downForceInAirAfterApex);
            }
            else
            {
                carRigidbody.AddForce(Vector3.down * downForceInAirBeforeApex);
            }
        }
        // Return if stunned
        if (IsStunned)
        {
            return;
        }
        // Air control when not stunned
        if (!IsGrounded)
        {
            AirControl();
            return;
        }
        carRigidbody.drag = 1f;

        //Fix for Slip Stream bugassa
        if (isSlipstreamed)
        {
            vFXHandler.PlaySpeedLines();
            CurrentMaxSpeed = slipStreamMaxSpeed;
            slipStreamTimer -= Time.deltaTime;
            if(slipStreamTimer <= 0)
            {
                vFXHandler.StopSpeedLines();
                //CurrentMaxSpeed = DefaultMaxSpeed + (slipStreamMaxSpeed - DefaultMaxSpeed);
                CurrentMaxSpeed = DefaultMaxSpeed;
                isSlipstreamed = false;
            }
        }
        
        // shit patch for slipstream bug
        //if(CurrentMaxSpeed < DefaultMaxSpeed && !IsOffRoading)
        if(CurrentMaxSpeed < DefaultMaxSpeed)
        {
            CurrentMaxSpeed = DefaultMaxSpeed;
        }
        //if (IsOffRoading && !isBoosting)
        //{
        //    CurrentMaxSpeed = OffRoadMaxSpeed;
        //}
        if (carRigidbody.velocity.magnitude < 2.5f)
        {
            carRigidbody.velocity = Vector3.zero;
        }
        DrivingLogic();
        LimitMaxForwardSpeed();

    }

    private void DrivingLogic()
    {
        UpdateMaxSpeed();
        TurningLogic();
        ForwardBackwardLogic();
    }
    private void ForwardBackwardLogic()
    {
        // print(carRelativeVelocity.z);
        

        if (Mathf.Abs(AccelerationInput) > 0.1f || isBoosting)
        {
            // If is boosting from drift, set speed strait to 1, else allow the player to feather the joystick to get and accel input between 0 and 1
            float _speed = (isBoosting) ? 1 : AccelerationInput;
            // Multiply speed by current max speed
            _speed *= CurrentMaxSpeed;
            // Multiply speed depending on the steepness of the slope the car is on
            _speed *= SlopeMultiplier();

            // Calculate the time it takes to reach the target speed
            float timeToMaxSpeed = _speed / currentAcceleration;
            // Calculate the fraction of time that has passed towards reaching the target speed
            // Clamp so t is always between 0 and 1.
            float t = Mathf.Clamp01(Time.deltaTime / timeToMaxSpeed);

            // Interpolate the current velocity towards the target velocity using Lerp with t as time between
            carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, transform.forward * _speed, t);
        }
        //reverse logic
        if (Mathf.Abs(BrakeInput) > 0.1f)
        {
            if (!LockedIntoDrift)
            {

                float _speed = BrakeInput;
                // Multiply speed by reverse max speed
                _speed *= reverseMaxSpeed;

                float timeToMaxSpeed = carRelativeVelocity.z > 0 ? reverseMaxSpeed / brakeAcceleration : reverseMaxSpeed / reverseAcceleration;
                float t = Mathf.Clamp01(Time.deltaTime / timeToMaxSpeed);
                carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, -transform.forward * _speed, t);
            }

            //If not drifting
            //if (!LockedIntoDrift)
            //{



            //}
        }

        //friction simulation (velocity dampening)
        Vector3 carVelocityXZ = new Vector3(carRigidbody.velocity.x, 0, carRigidbody.velocity.z).normalized;
        Vector3 carRightXZ = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        float dot = Vector3.Dot(carVelocityXZ, carRightXZ);
        carRigidbody.AddForce(transform.right * -dot * dragMultiplier);
    }    
    private void TurningLogic()
    { 
        //print(carRelativeVelocity.z / CurrentMaxSpeed);
        float _turnCurveMultiplier = turnCurve.Evaluate(Mathf.Abs(carRelativeVelocity.z / CurrentMaxSpeed)); // divide by CurrentMaxSpeed so it goes between 0 and 1
        //_turnCurveMultiplier *= Mathf.Abs(carRelativeVelocity.z);
        _turnCurveMultiplier *= 100f;
        //if driving forwards
        if (carRelativeVelocity.z > 2.5f)
        {
            //if drifting
            if (LockedIntoDrift)
            {
                //add torque based on the DriftSteerInput and the driftTurnMultiplier 
                carRigidbody.AddTorque(Vector3.up * DriftSteerInput * driftTurnTorque * driftTorqueMulitplier * _turnCurveMultiplier);
                //add sideways force against the player to make them slide out whilst drifting
                //carRigidbody.AddForce(transform.right * -steeringSign * driftSidewaysForce * driftSidewaysForceMultiplier * AccelerationInput, ForceMode.Acceleration);
                carRigidbody.AddForce(transform.right * -steeringSign * driftSidewaysForce * driftSidewaysForceMultiplier * carRelativeVelocity.z, ForceMode.Acceleration);
                
            }
            else
            {
                carRigidbody.AddTorque(Vector3.up * SteerInput * forwardTurnMultiplier * _turnCurveMultiplier);
            }
        }
        //driving backwards
        else if(carRelativeVelocity.z < -2.5f)
        {
            // Check if the player initiated the J-turn
            if (LockedIntoDrift)
            {
                // Reverse the car's movement
                float _speed = BrakeInput * reverseMaxSpeed;
                // Multiply speed by reverse max speed
                float timeToMaxSpeed = reverseMaxSpeed / reverseAcceleration;
                float t = Mathf.Clamp01(Time.deltaTime / timeToMaxSpeed);

                // Apply steering input
                carRigidbody.AddTorque(Vector3.up * -1 * SteerInput * reverseTurnMultiplier * _turnCurveMultiplier);

                // Check if the car is drifting
                if (carRigidbody.angularVelocity.magnitude > jTurnDriftThreshold)
                {
                    //isDriftingReverse = true;

                    _speed = BrakeInput * jturnspeed;
                    carRigidbody.AddTorque(Vector3.up * -1 * spinForce * 100f * SteerInput);
                }
                carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, -transform.forward * _speed, t);

            }
            else
            {
                carRigidbody.AddTorque(Vector3.up * -1 * SteerInput * reverseTurnMultiplier * _turnCurveMultiplier);
            }            
        }
    }    
    private void DriftInputHandling()
    {
        //If holding drift input and turning and hasn't been bumped whilst drifting
        if (HandBrakeInput && Mathf.Abs(SteerInput) > 0.1f && !bumpedWhilstDrifting && IsGrounded)
        {
            //If not locked into drift, locked into drift is true so this is called once
            if (!LockedIntoDrift)
            {
                LockedIntoDrift = true;
                //steering directional sign is locked and can't be changed until the player starts a new drift
                steeringSign = Mathf.Sign(SteerInput);

                //rumbleController.PlayRumble(0.5f, 1f, 0.5f);
            }
        }
        if (LockedIntoDrift)
        {
            driftSidewaysForceMultiplier = driftSidewaysMultiplierCurve.Evaluate(driftTimer);
            driftTorqueMulitplier = driftTorqueMultiplierCurve.Evaluate(driftTimer);

            //increase drift timer (used for speed boosts) if grounded
            if (IsGrounded && (AccelerationInput > 0.1f || BrakeInput > 0.1f))
            {
                //Increase drift timer
                IncreaseDriftTimer();
            }

            //evaluate the drift curve
            //-1 steer input corresponds to 0.5 on the curve so if the player is drifting right but holding left, the driftSteeringInput will be 0.5 not -1
            //1 steer input corresponds to 1.5, on the curve so if the player is drifting right and holding right, the driftSteeringInput will be 1.5 not 1
            //evaluating the curve with the steer input multiplied by the sign will invert these results
            DriftSteerInput = driftInputCurve.Evaluate(SteerInput * steeringSign);
            //multiplying the results by the steering sign will make the car drift
            DriftSteerInput *= steeringSign;
        }
        //If player is not holding drift button
        if (!HandBrakeInput)
        {
            if (LockedIntoDrift)
            {
                DriftComplete();
            }
            driftTimer = 0f;
            steeringSign = 0f;
            DriftSteerInput = 0f;
            LockedIntoDrift = false;
            bumpedWhilstDrifting = false;
        }
    }
    private void GroundCheck()
    {
        //the start of the ray will be above the car to prevent going through the floor, since were apllying a down force
        //ray shoots worldspace down, if it hits, the cars position will be offset in world space up
        Vector3 rayStartPosition = transform.position + StartRayOffset;
        Vector3 rayDirection = -Vector3.up;
        float rayDistance = centerGroundedDistance + StartRayOffset.y;

        Debug.DrawRay(rayStartPosition, rayDirection * rayDistance, Color.red);

        if (Physics.Raycast(rayStartPosition, rayDirection, out centreHit, rayDistance, DrivableSurfaceMask))
        {
            if (Vector3.Angle(centreHit.normal, Vector3.up) < maxSteepness)
            {
                // Get the layer of the hit object
                //IsOffRoading = centreHit.collider.CompareTag("OffRoad");
                IsGrounded = true;
                return;
            }
        }
        IsGrounded = false;
    }
    void GroundCheckWheels()
    {
        //each wheel cast a ray locally down from it's centre (or higher)
        //if the ray hits a surface the wheel will be driving on that point
        //an invisible plane will be made with all the points
        //the car will be rotated to the normal of that plane (if 4 wheels are grounded?)
        groundhits.Clear();        
        for (int i = 0; i < 4; i++)
        {
            Vector3 startPos = allWheelHolders[i].position + transform.up * StartRayOffset.y;
            Vector3 rayDirection = -transform.up;
            float rayDistance = wheelGroundedDistance + StartRayOffset.y;
            //Debug.DrawRay(startPos, rayDirection * rayDistance, Color.blue);
            if (Physics.Raycast(startPos, rayDirection, out WheelRayHits[i], rayDistance, DrivableSurfaceMask))
            {
                if (Vector3.Angle(WheelRayHits[i].normal, Vector3.up) < maxSteepness)
                {
                    groundhits.Add(WheelRayHits[i].point);
                    WheelIsGrounded[i] = true;

                }
                else
                {
                    WheelIsGrounded[i] = false;
                }
            }
            else
            {
                WheelIsGrounded[i] = false;
            }
        }
    }
    Vector3 normalFromPlaneUnderWheels;
    private void RotateToSurface()
    {
        for (int i = 0; i < 4; i++)
        {
            if (WheelIsGrounded[i])
            {
                //stores the hit point below the wheel
                wheelHitPositions[i] = WheelRayHits[i].point;
            }
            else
            {
                //if the wheel isn't grounded, it gets a point locally down from the wheel
                wheelHitPositions[i] = allWheelHolders[i].position + (transform.up * -wheelGroundedDistance);
            }
        }
        //essentially makes a plane with all the points locally down from the wheel
        Vector3 a = wheelHitPositions[0];
        Vector3 b = wheelHitPositions[1];
        Vector3 c = wheelHitPositions[2];
        Vector3 d = wheelHitPositions[3];
        Vector3 x = c - a + d - b;
        Vector3 y = c - d + a - b;

        //calculates the normal
        normalFromPlaneUnderWheels = Vector3.Cross(x, y);

        //rotates car to that normal
        carRigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, normalFromPlaneUnderWheels) * transform.rotation, rotateToSurfaceSpeed));

        //set the car's position to the ray cast hit below + an offset
        if (IsGrounded)
            transform.position = new Vector3(transform.position.x, centreHit.point.y + YOffset, transform.position.z);
    }
    float rotateToSurfaceSpeed = 0.2f; // used to be 0.8 but 0.2 is ssooooooo much smoother
    private void AirControl()
    {
        //reset the car's rotation from it's local up vector to the world's up vector
        //doesn't need to slerp by a time delta time value coz in fixed update

        float sidewayVelocity = carRelativeVelocity.x + SteerInput * airAcceleration;
        carRelativeVelocity.x = Mathf.Clamp(sidewayVelocity, -airMaxSidewaysSpeed, airMaxSidewaysSpeed);
        carRigidbody.velocity = transform.TransformDirection(carRelativeVelocity);
    }
    private float SlopeMultiplier()
    {
        //Calculate the y-component product of the transform's up and forward vectors
        //backforce depending on slopes (cars transform up is rotated to) and which way the car is facing
        //if the car is on a 45 degree slope and driving up, this number will be 0.5
        //if it's driving down it will be -0.5
        //if its driving parallel, it'll be 0
        float _yProduct = transform.up.y * transform.forward.y;
        return slopeMultiplierCurve.Evaluate(_yProduct);
    }
    private void IncreaseDriftTimer()
    {
        driftTimer += Time.deltaTime;

        // Iterate through the timesForDifferentDriftBoosts array to find the maximum BoostLevel
        for (int i = 0; i < driftBoostThresholds.Length; i++)
        {
            // If the driftTimer is greater than the current boost threshold's time, update BoostLevel
            if (driftTimer > driftBoostThresholds[i])
            {
                CurrentBoostLevel = i + 1; // BoostLevel is one more than the current index, since arrays are zero-indexed
                //GamepadRumbler.SetCurrentGamepad(PlayerID);
                //HapticPatterns.PlayConstant(CurrentBoostLevel * 0.1f, 0.75f, 1.3f);
                //rumbleController.PlayRumble((CurrentBoostLevel + 1) * 0.1f, (-1 + 2 * CurrentBoostLevel), 1.3f); //Change this so one shot is played every certain number of frames instead 
            }
        }
    }
    private void DriftComplete()
    {
        if (CurrentBoostLevel > 0)
        {
            float speed_increase = driftBoostSpeeds[CurrentBoostLevel - 1];
            //float accel_increase = driftBoostAccelerations[CurrentBoostLevel - 1];
            float accel_increase = boostDefualtAccelerationIncrease;
            float duration = driftBoostDurations[CurrentBoostLevel - 1];
            driftType = CurrentBoostLevel-1;
            initialDriftTime = Time.time;
            //StartCoroutine(SpeedBoost(speed_increase, boostDefualtAccelerationIncrease, duration));
        }
        // Reset values
        driftTimer = 0;
        CurrentBoostLevel = 0;
        /*
        HapticController.Loop(false);
        HapticController.clipLevel = 0.25f;
        HapticController.clipFrequencyShift = 0.0f;
        HapticController.Stop();
        */
    }
    private void DriftCancel()
    {
        //reset values
        bumpedWhilstDrifting = true;
        LockedIntoDrift = false;
        driftTimer = 0;
        CurrentBoostLevel = 0;
    }
    float initialBoostPanelTime;
    public void BoostPanelEnter()
    {
        initialBoostPanelTime = Time.time;   
    }
    int driftType = 0;
    float initialDriftTime = 0;

    private void UpdateMaxSpeed()
    {
        currentAcceleration = defaultAcceleration;
        CurrentMaxSpeed = DefaultMaxSpeed;
        //Checks for slipstream
        if (isSlipstreamed)
        {
            vFXHandler.PlaySpeedLines();
            CurrentMaxSpeed = slipStreamMaxSpeed;
            slipStreamTimer -= Time.deltaTime;
            if (slipStreamTimer <= 0)
            {
                //Checks to see if a boos is active
                if(!isBoosting) vFXHandler.StopSpeedLines();
                //CurrentMaxSpeed = DefaultMaxSpeed + (slipStreamMaxSpeed - DefaultMaxSpeed);
                CurrentMaxSpeed = DefaultMaxSpeed;
                isSlipstreamed = false;
            }
        }

        bool checkBoosting = false;
        if (initialBoostPanelTime + boostPanelDuration > Time.time)
        {
            currentAcceleration += boostPanelAccelerationIncrease;
            CurrentMaxSpeed += boostPanelSpeedIncrease;
            checkBoosting = true;
        }

        if (initialPowerUpBoostTime + powerUpBoostDuration > Time.time)
        {
            currentAcceleration += powerUpBoostAccelerationIncrease;
            CurrentMaxSpeed += powerUpBoostSpeedIncrease;
            checkBoosting = true;
        }

        if (driftType >=0 && driftType <= 3)
        {
            if (initialDriftTime + driftBoostDurations[driftType] > Time.time)
            {
                currentAcceleration += driftBoostAccelerations[driftType];
                CurrentMaxSpeed += driftBoostSpeeds[driftType];
                checkBoosting = true;
            }
        }

        if (!checkBoosting && isBoosting)
        {
            SpeedBoostVisualDisable();
        }
        else if(checkBoosting && !isBoosting)
        {
            SpeedBoostVisualEnable();

        }            

    }
    [SerializeField] float jumpForce;
    public void JumpFunction()
    {
        carRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public void BoostPowerUpFunction()
    {
        //AnalyticsManager.instance.boostItemsUsed++;
        //StartCoroutine(SpeedBoost(powerUpBoostSpeedIncrease, powerUpBoostAccelerationIncrease, powerUpBoostDuration));

        initialPowerUpBoostTime = Time.time;
    }
    int currentBoosts = 0;
    IEnumerator SpeedBoost(float speedIncreaseAmount, float accelIncreaseAmount, float boostDuration)
    {
        currentBoosts++;
        CurrentMaxSpeed += speedIncreaseAmount;
        currentAcceleration += accelIncreaseAmount;
        SpeedBoostVisualEnable();

        yield return new WaitForSeconds(boostDuration);
        currentBoosts--;
        currentAcceleration -= accelIncreaseAmount;
        CurrentMaxSpeed -= speedIncreaseAmount;

        if(currentBoosts == 0)
        {
            SpeedBoostVisualDisable();
        }
    }

    private void SpeedBoostVisualEnable()
    {
        isBoosting = true;
        GetComponentInChildren<CameraManager>().IncreaseFieldOfView();
        vFXHandler.PlaySpeedLines();
        vFXHandler.PlayBoostExhaustFlame();
        carVisualsHandler.BodyScaleSpring(new Vector3(5.0f, 0.5f, 5.0f));
    }

    private void SpeedBoostVisualDisable()
    {
        isBoosting = false;
        vFXHandler.StopSpeedLines();
        vFXHandler.StopBoostExhaustFlame();
    }

    [SerializeField] float stunForce = 50f;
    public void StunnedActionFunction(bool spin, Vector3 stunDirection, float duration) 
    {
        StartCoroutine(StunnedAction(spin, stunDirection, duration)); 
    }
    IEnumerator StunnedAction(bool spin, Vector3 stunDirection, float duration)
    {
        if (!playerStatus.Invincible)
        {
            carRigidbody.velocity = carRigidbody.velocity / 2;

            IsStunned = true;

            //rumbleController.PlayOneShot(0.5f, 0.5f);   //Heavy is a bit much but Medium is not enough :O
            // Called when attacked
            if (spin)
            {
                carRigidbody.AddForce(stunDirection * stunForce, ForceMode.Impulse);
                float numberOfRotations = 3;
                float elapsedTime = 0.0f;
                carVisualsHandler.BodyScaleSpring(new Vector3(8, 0.1f, 8));
                vFXHandler.PlayStunEffect();
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float angle = Mathf.Lerp(0.0f, 360.0f * numberOfRotations, elapsedTime / duration);
                    carBodyHolder.localRotation = Quaternion.Euler(transform.GetChild(0).rotation.x, angle, transform.GetChild(0).rotation.z);
                    yield return null;
                }
                carBodyHolder.localRotation = Quaternion.Euler(transform.GetChild(0).rotation.x, 0, transform.GetChild(0).rotation.z);
            }
            // Called when bumped into springy tree
            else
            {
                yield return new WaitForSeconds(duration);
            }
            IsStunned = false;
        }
    }
    public void SlowToStopFunction()
    {
        StartCoroutine(SlowToStop());
    }
    IEnumerator SlowToStop()
    {
        SpeedBoostVisualDisable();
        DriftCancel();

        playerStatus.IsStopping = true;
        Vector3 startVelocity = carRigidbody.velocity;
        Vector3 targetVelocity = carRigidbody.velocity * 0f;
        float elapsedTime = 0.0f;
        float slowDownDuration = 1.5f;
        while (elapsedTime < slowDownDuration)
        {
            elapsedTime += Time.deltaTime;
            carRigidbody.velocity = Vector3.Lerp(
                startVelocity, targetVelocity, elapsedTime / slowDownDuration);
            yield return null;
        }
        carRigidbody.velocity = Vector3.zero; // Ensure the velocity becomes exactly zero
        playerStatus.IsStopping = false;
    }
    [SerializeField] float UTurnDuration = 0.8f;
    [SerializeField] float UTurnTargetSpeed = 15f;
    private void OnTriggerStay(Collider other)
    {
        //if other collider is not tagged as SlipStreamTrigger, return
        //if (other.transform.root != transform && other.transform != transform)

        if (other.CompareTag("SlipStreamTrigger") && other.transform.root.GetComponent<ArcadeVehicleController>().IsEmmittingTrail == true)
        {
            //Fix for slip stream bug
            isSlipstreamed = true;
            slipStreamTimer = 1f;
        }        
    }
    void LimitMaxForwardSpeed()
    {
        if(carRelativeVelocity.z > MaxZVelocity)
        {
            carRigidbody.velocity = transform.TransformDirection(new Vector3(carRelativeVelocity.x, carRelativeVelocity.y, MaxZVelocity));
        }
    }

    [SerializeField] float backForceFromSpringyObstacle = 75f;
    [SerializeField] float timeSinceLastCollision = 0f;
    private void OnCollisionEnter(Collision collision)
    {
        Transform collisionTransform = collision.transform;
        // Play audio depending on the collision
        PlayCollisionAudio(collisionTransform);
        // drift should get cancel if LockedIntoDrift and not hitting a pedestrian car
        bool shouldCancelDrift = false;
        bool shouldCancelBoost = false;

        /*
        if (LockedIntoDrift && collisionTransform.root.GetComponent<PedestrianScriptCarReal>() == false && !collisionTransform.CompareTag("Obstacle"))
        {
            shouldCancelDrift = true;
        }
        if (collisionTransform.root.GetComponent<PedestrianScriptCarReal>() == false && !collisionTransform.CompareTag("Obstacle"))
        {
            shouldCancelBoost = true;
        }
        */

        if (collisionTransform.TryGetComponent<ObstacleManager>(out ObstacleManager obstacle))
        {
            if (obstacle.type == ObstacleManager.ObjectType.springy)
            {
                HitSpringyObstacle(collision);
            }
        }

        // If drifting and not a pedestrian car, cancel drift
        if (shouldCancelDrift)
        {
            DriftCancel();
        }
        if (shouldCancelBoost)
        {
            SpeedBoostVisualDisable();
        }

    }
    [SerializeField] float minCarSpeed = 20f;
    private void PlayCollisionAudio(Transform collisionTransform)
    {
        if (timeSinceLastCollision < 1f || carRelativeVelocity.z < minCarSpeed)
        {
            return;
        }
        //if (collisionTransform.TryGetComponent<ObstacleManager>(out ObstacleManager obstacle))
        //{
        //    //carAudioHandler.PlayObstacleSound(obstacle.AudioClip);
        //}
        else if (collisionTransform.CompareTag("PedestrianCar"))
        {
            carAudioHandler.PlayPedestrianCarSound();
        }
        else if (collisionTransform.root.CompareTag("Player"))
        {
            carAudioHandler.PlayOtherPlayerCarSound();
        }
        else
        {
            carAudioHandler.PlayDefaultCrashSound();
        }
        timeSinceLastCollision = 0f;
    }

    private void HitSpringyObstacle(Collision collision)
    {
        StartCoroutine(StunnedAction(false, Vector3.zero, 0.2f));

        //get the car veloticty and normalize it
        Vector3 carveloticty = new Vector3(carRigidbody.velocity.x, 0, carRigidbody.velocity.z).normalized;
        // get the reflected vector from the car velocity and the contact moraml
        Vector3 newDir = Vector3.Reflect(carveloticty, collision.GetContact(0).normal);

        float x = Mathf.Deg2Rad * Vector3.Angle(transform.forward, -collision.GetContact(0).normal);

        x = Mathf.Abs(Mathf.Cos(x));
        //x will be 1 if hit obstacle head on, 0 if hit from side

        //Debug.DrawRay(transform.position, carveloticty, Color.blue, 7);
        //Debug.DrawRay(collision.contacts[0].point, newDir, Color.red, 7);
        //Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 7);

        carRigidbody.velocity = Vector3.zero;
        carRigidbody.AddForce(x * newDir * (collision.relativeVelocity.magnitude * backForceFromSpringyObstacle));
    }
    private IEnumerator HornDelay()
    {
        yield return new WaitForSeconds(hornDelay);
        canHorn = true;
    }
}
