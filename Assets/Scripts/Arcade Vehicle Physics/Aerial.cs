//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;

public class Aerial : MonoBehaviour
{
    public Transform springTarget;
    public Transform springObj;

    
    public float drag = 2.5f;//drag
    public float springForce = 80.0f;//Spring

    
    public Transform holder;

    Rigidbody SpringRB;
    private Vector3 LocalDistance;//Distance between the two points
    private Vector3 LocalVelocity;//Velocity converted to local space

    void Start()
    {
        SpringRB = springObj.GetComponent<Rigidbody>();//Find the RigidBody component
        //springObj.transform.parent = null;//Take the spring out of the hierarchy
    }

    void FixedUpdate()
    {
        //Sync the rotation 
        SpringRB.transform.rotation = this.transform.rotation;

        //Calculate the distance between the two points
        LocalDistance = springTarget.InverseTransformDirection(springTarget.position - springObj.position);
        SpringRB.AddRelativeForce((LocalDistance) * springForce);//Apply Spring

        //Calculate the local velocity of the springObj point
        LocalVelocity = (springObj.InverseTransformDirection(SpringRB.velocity));
        SpringRB.AddRelativeForce((-LocalVelocity) * drag);//Apply drag

        //Aim the visible geo at the spring target
        holder.transform.LookAt(springObj.position, new Vector3(0, 0, 1));
    }
}
