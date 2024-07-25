using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;

public class PedestrianScriptCarReal : MonoBehaviour
{
    #region create variables
    [SerializeField] Material[] materials;
    [SerializeField] GameObject[] carBodies;
    private SpringToScale scaleSpring;
    [SerializeField] TrafficRoute route;                //The parent route that the designer can drag in
    [SerializeField] TrafficNodeData currentNode;       //The information relating to current waypoint
    [SerializeField] float downForce;
    [SerializeField] float collisionMultiplier;

    private int currentIndex;            //The current way point tracker

    [SerializeField] float m_speed;                     //The speed of the cars
    private float m_angleOffset;                        //Defines the max angle the car can turn at one time.
    private float m_waypointDistanceOffset;             //Define the distance it begins to turn.
    private float m_rebootTimer;                        //Define the time it takes to reset car functionality.

    private bool m_hit;                                 //Checks to see if the car has been hit
    public bool GetIsHit() { return m_hit; }
    private bool isGrounded;                            //Checks to see if the car is on ground
    private bool somethingInFront;                     //Checks to see if the car has no object infront
    private Vector3 m_currentTargetPos;                 //Defines the current waypoint target 
    private Rigidbody rb;

    [SerializeField] LayerMask drivableSurface;
    #endregion
    public void SetRoute(TrafficRoute i)
    {
        route = i;
        currentNode = route.childNodes[0];
    }
    public void SetNode(int i)
    {
        currentIndex = i;
        currentNode = route.childNodes[i];
    }
    
    void Start()
    {
        #region intialise variables   
        SetupCarBody();
        rb = GetComponent<Rigidbody>();
        m_speed = 30.0f;
        m_angleOffset = 1.75f;
        m_waypointDistanceOffset = 20f;
        m_rebootTimer = 3.0f;
        m_hit = false;
        //set material to random material in array
        
        #endregion
    }
    private void SetupCarBody()
    {
        for (int i = 0; i < carBodies.Length; i++)
        {
            carBodies[i].SetActive(false);
        }
        int randIndex = Random.Range(0, carBodies.Length);
        carBodies[randIndex].SetActive(true);
        carBodies[randIndex].GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
        scaleSpring = carBodies[randIndex].GetComponent<SpringToScale>();
    }
    private void ApplySteer()
    {
        #region Steering method
        //Get the position of the waypoint.
        Vector3 targetDir = route.childNodes[currentIndex].getPos - transform.position;
        //Calculate the rotation increments per frame.
        float singleStep = m_angleOffset * Time.deltaTime;
        //Calculate the new direction.
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);
        //Apply transformation.
        transform.rotation = Quaternion.LookRotation(newDir);
        #endregion
    }
    private void Drive()
    {
        #region drive method
        //Apply movement transformation
        transform.position += transform.forward * m_speed * Time.deltaTime;
        #endregion
    }
    private void checkNodes()
    {
        #region check nodes and update
        //Check if the car has reached the final node in the route.
        if (currentIndex == route.childNodes.Count - 1)
        {
            //Loop.
            currentIndex = 0;
        }
        //If the car is within a suitable distance - move to the next waypoint.
        if (Vector3.Distance(transform.position, route.childNodes[currentIndex].getPos) < m_waypointDistanceOffset)
        {
            currentIndex++;
        }
        #endregion
    }
    IEnumerator waitForCar(float distInfront)
    {
        somethingInFront = true;
        yield return new WaitForSeconds(3.0f);
        somethingInFront = false;
    }
    private void checkInFront()
    {
       
        float distInfront = 30.0f;
        RaycastHit hit;

        somethingInFront = false;

        //Debug.DrawRay(transform.position - new Vector3(0.0f, 1.0f, 0.0f), transform.forward * distInfront, Color.red);
        if (Physics.Raycast(transform.position - new Vector3(0.0f, 1.0f, 0.0f), transform.forward, out hit, distInfront, drivableSurface))
        {
            if (hit.collider.CompareTag("PedestrianCar"))
            {
                
                somethingInFront = true;
                //If the raycast is sucessful
                //StartCoroutine(waitForCar(distInfront));
            }
        }

        //Debug.DrawRay(transform.position - new Vector3(0.0f, 1.0f, 0.0f), transform.forward * distInfront, Color.red);
    }
    [SerializeField] float lerpSpeed;
    [SerializeField] float heightOffGround;
    private void adjustOrientationAndGroundCheck()
    {
        #region adjust orientation method
        //Set up raycast
        RaycastHit hit;
        //Set up distance for ray to cast
        float distToGround = 6;
        //If the raycast is sucessful
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround, drivableSurface))
        {
            isGrounded = true;
            //Transform the rotation of the object to be parrallel to the surface it hits.
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

           // transform.position = hit.point + new Vector3(0, heightOffGround, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);
        }
        else
        {
            isGrounded = false;
        }
        #endregion
    }
    [SerializeField] float upforce;
    [SerializeField] float playerVelocityMultiplier;
    private void OnCollisionEnter(Collision collision)
    {
        
        #region collision method
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody other_rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = other_rb.velocity * collisionMultiplier;
            rb.AddForce(Vector3.up * upforce, ForceMode.VelocityChange);
            other_rb.velocity *= playerVelocityMultiplier;
            scaleSpring.Nudge(new Vector3(15.0f, 2.0f, 15.0f));
            //Get the rigidbody of the car
            //Apply force relative to the onject that collided with it
            //other_rb.velocity = collision.relativeVelocity;
          
            //Set the hit status to true.
            m_hit = true;
        }
        #endregion
    }
    IEnumerator rebootCar()
    {
        //Disable car controls for x seconds.
        yield return new WaitForSeconds(m_rebootTimer);
        m_hit = false;
    }
    void Update()
    {
        //If the car is not hit.
        if (!m_hit)
        {
            //Execute car functions.
           
            if (isGrounded && !somethingInFront)
            {
                checkNodes();
                ApplySteer();
                Drive();

            }
            else
            {
                 if(m_hit) StartCoroutine(rebootCar());
            }
        }
        else
        {
            //Disable car for x seconds.
            if (m_hit) StartCoroutine(rebootCar());
        }
    }

  

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * downForce, ForceMode.VelocityChange);
        if (!m_hit)
        {
            checkInFront();
            adjustOrientationAndGroundCheck();
        }
    }
}
