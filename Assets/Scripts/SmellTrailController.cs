using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellTrailController : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public bool trailOn;
    [SerializeField] private float additionalLength;
    [SerializeField] private GameObject trailCollisionHolder;
    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private int positionCount;
    [SerializeField] private int boxColliderAmount = 20;
    [SerializeField] float float_rate;
    [SerializeField] int int_rate;
    private ArcadeVehicleController arcadeVehicleController;

    [SerializeField] Material smellTrailMat1;
    [SerializeField] Material smellTrailMat2;

    [SerializeField] float trail1Speed;
    [SerializeField] float trail2Speed;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        arcadeVehicleController = transform.root.GetComponent<ArcadeVehicleController>();
        DisableTrail();

        AddBoxColliders();        
    }

    private void AddBoxColliders()
    {
        for (int i = 0; i < boxColliderAmount; i++)
        {
            //Create a new empty gameobject 
            GameObject collider = new GameObject("TrailCollider: " + i);
            //Parent it to the collision holder object to keep the heirachy neat
            collider.transform.parent = trailCollisionHolder.transform;
            //set the layer to be the same as the root layer so the car's ground check raycast ignores it
            collider.layer = trailCollisionHolder.gameObject.layer;
            //Add a box collider to the object and set it to be a trigger
            collider.AddComponent<BoxCollider>();
            collider.GetComponent<BoxCollider>().isTrigger = true;
            //Set the tag to SlipStreamTrigger so player can recognise it
            collider.tag = "SlipStreamTrigger";
        }
        boxColliders = trailCollisionHolder.GetComponentsInChildren<BoxCollider>();
    }

    private void Update()
    {

        //update the positions of the box colliders if they're rendering
        if (trailRenderer.emitting)
        {
            UpdateBoxColliders();
            smellTrailMat1.SetFloat("mainTexSpeed", -arcadeVehicleController.carRelativeVelocity.z * trail1Speed);
            smellTrailMat2.SetFloat("mainTexSpeed", -arcadeVehicleController.carRelativeVelocity.z * trail2Speed);
        }
    }
    void SetCollidersToEnabled(bool b)
    {
        foreach (var boxCol in trailCollisionHolder.GetComponentsInChildren<BoxCollider>())
        {
            boxCol.enabled = b;
        }
    }
    private void UpdateBoxColliders()
    {
        // Get the number of positions in the trail renderer
        positionCount = trailRenderer.positionCount;

        // If there are no positions, return
        if (positionCount < 1)
        {
            return;
        }

        // Calculate the rate at which to place box colliders along the trail
        float_rate = positionCount / boxColliderAmount;

        // Round down the rate to an integer
        int_rate = Mathf.FloorToInt(float_rate);

        // Loop through the number of box colliders
        for (int i = 0; i < boxColliderAmount; i++)
        {
            // Initialize variables for the start and end positions of the current box collider
            Vector3 endPos;
            Vector3 startPos;

            // Get the transform of the current box collider
            Transform boxCol = trailCollisionHolder.transform.GetChild(i);

            // Calculate the start position of the current box collider based on the rate and make sure the index isn't outside the array
            if ((int_rate * i) < trailRenderer.positionCount)
            {
                startPos = trailRenderer.GetPosition(int_rate * i);
            }
            else
            {
                startPos = trailRenderer.GetPosition(positionCount - 1 - int_rate);
            }
            // Calculate the start position of the current box collider based on the rate and make sure the index isn't outside the array
            if ((int_rate * i) + int_rate < trailRenderer.positionCount)
            {
                endPos = trailRenderer.GetPosition((int_rate * i) + int_rate);
            }
            else
            {
                endPos = trailRenderer.GetPosition(positionCount - 1);
            }

            // Calculate the center position and direction of the current box collider
            Vector3 centerPos = (startPos + endPos) / 2f;
            Vector3 dir = (endPos - startPos).normalized;

            // Set the position, rotation, and scale of the current box collider
            boxCol.transform.position = centerPos;
            boxCol.transform.LookAt(boxCol.transform.position + dir);
            boxCol.localScale = new Vector3(7.5f, 7.5f, Vector3.Distance(endPos, startPos));
            //boxCol.localScale = new Vector3(6f, 6f, Vector3.Distance(endPos, startPos));
        }
    }

    public void EnableTrail()
    {
        trailRenderer.emitting = true;
        arcadeVehicleController.IsEmmittingTrail = true;
        //set the colliders to enabled depending on whether the trail renderer is emitting
        SetCollidersToEnabled(trailRenderer.emitting);
    }
    public void DisableTrail()
    {
        arcadeVehicleController.IsEmmittingTrail = false;
        trailRenderer.emitting = false;
        //set the colliders to enabled depending on whether the trail renderer is emitting
        SetCollidersToEnabled(trailRenderer.emitting);
    }
}
