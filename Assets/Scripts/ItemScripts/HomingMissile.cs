using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HomingMissile : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Rigidbody target;    //How to find who has active delivery?    
    [SerializeField] private ParticleEffectGroup expolsionEffectGroup;
    [SerializeField] private ParticleEffectGroup smokeEffectGroup;
    [SerializeField] AudioSource myAudioSource;

    float targetDist;

    [Header("Movement")]
    [SerializeField] float speed = 120;
    [SerializeField] float rotateSpeed = 150;

    [Header("Prediction")]
    float maxDistPredict = 150;
    float minDistPredict = 5;
    float maxTimePrediction = 2;
    Vector3 prediction, deviatedPrediction;

    LockOnIcon targetLockOn;

    bool trackTarget;
    public Rigidbody firingPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();        
        target = FindTarget();
        StartTargetLockOn(target);

        //Spherecast to get rigidbody with certain range


        trackTarget = false;

        //After 2s, start tracking the target and rotating the missile
        Invoke("StartTrackTarget", 0.9f);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;

        targetDist = Vector3.Distance(transform.position, target.position);
        var leadTimePercentage = Mathf.InverseLerp(minDistPredict, maxDistPredict, targetDist);

        targetLockOn.missileDistance = targetDist;

        if (trackTarget)
        {
            PredictMovement(leadTimePercentage);
            //AddDeviation(leadTimePercentage);
            RotateRocket();
        }
    }

    private void OnCollisionEnter(Collision other)
    {        
        Explode();
    }

    private void StartTrackTarget()
    {
        trackTarget = true;
        speed += 30;
    }

    private Rigidbody FindTarget()
    {
        Rigidbody potentialTarget;
        foreach (Transform car in TestScene01Manager.instance.PlayerCars)
        {
            if (car.GetComponent<Delivery>().PlayerHasItem())
            {
                potentialTarget = car.GetComponent<Rigidbody>();

                if (potentialTarget != firingPlayer)
                {
                    //return DeliveryManager.instance.GetSecondPlacePlayer();
                    return potentialTarget;
                }                
            }
        }
        potentialTarget = DeliveryManager.instance.GetLeadingPlayerExcludingSelf(firingPlayer);
        return potentialTarget;
    }
    private void StartTargetLockOn(Rigidbody target)
    {
        targetLockOn = target.GetComponentInChildren<LockOnIcon>();
        targetLockOn.lockedOn = true;
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);
        prediction = targetDist > 10f ? target.position + target.velocity * predictionTime : target.position;

    }

    private void RotateRocket()
    {
        var heading = prediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    private void Explode()
    {
        smokeEffectGroup.Stop();
        Collider[] colliders = new Collider[4];
        int numberOfCollision;
        numberOfCollision = Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.z * 8f, colliders, 1 << 6);
        for (int i = 0; i < numberOfCollision; i++)
        {
            if (colliders[i].TryGetComponent<ArcadeVehicleController>(out ArcadeVehicleController arcadeVehicleController))
            {
                arcadeVehicleController.StunnedActionFunction(true, Vector3.up, 1.5f);
            }
        }
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;

        expolsionEffectGroup.Play();
        expolsionEffectGroup.transform.SetParent(null);

        myAudioSource.Play();

        targetLockOn.lockedOn = false;

        Invoke("DestroySelf", 0.25f);
    }

    private void DestroySelf()
    {        
        Destroy(gameObject, 2.5f);
    }
}
