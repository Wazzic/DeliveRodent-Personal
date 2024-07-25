using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCarTest : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] float downDist;
    [SerializeField] float downForce;
    bool grounded;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 startRayOffset = new Vector3(0,50,0);
        Vector3 rayStartPosition = transform.position + startRayOffset;
        Vector3 rayDirection = Vector3.down;
        float rayDistance = downDist + startRayOffset.y;

        Debug.DrawRay(rayStartPosition, rayDirection * rayDistance, Color.red);
        if (Physics.Raycast(rayStartPosition, rayDirection, out hit, rayDistance))
        {
            if (!grounded)
            {
                //called once
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.useGravity = false;
                grounded = true;
            }
            
        }
        else
        {
            if (grounded)
            {
                rb.useGravity = true;
                grounded = false;
            }            
        }
        rb.AddForce(Vector3.up * -downForce);
    }
    void Update()
    {
       
        if (grounded)
        {
            transform.position = hit.point + new Vector3(0, transform.localScale.y / 2, 0);
        }
    }
}
