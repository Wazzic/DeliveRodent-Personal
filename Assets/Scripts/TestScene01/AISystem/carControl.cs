using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    TODO - Create very basic implementation of AI car.
    Set enums for finite state machine approach.
    States: Crusing, Dodging, Seeking Delivery, Seeking Enemy, Attacking, Delivering, Stunned.
    Transition Requirements:
    A = There are no current deliveries available.  - matched
    B = A delivery has appeared that is unclaimed.  - matched
    C = A delivery has been collected by me.        - matched
    D = A delivery has been collected by the enemy. - matched
    E = I have located the enemy with the delivery. - matched
    F = The enemy has located me with the delivery. - matched
    G = I have sucessfully delivered the food.      - matched
    H = I have ran out of time to deliver the food. - matched
    I = The enemy has delivered the food.           - matched
    J = I have sucessfully stole the food from the enemy. - matched
    K = I have been hit by the enemy.               - matched
    L = I am no longer stunned.                     
 */

public class carControl : MonoBehaviour
{
    #region variable intitalization and start function
    //States available to driver
    public enum driverStates
    {
        Cruising = 0,
        SeekingDelivery,
        SeekingEnemy,
        AttemptingToDeliver,
        Delivered,
        Attacking,
        Dodging,
        Stunned,
        DumpFood
    }
    //Destination for AI.
    [SerializeField] Transform[] destinations;
    [SerializeField] Transform[] deliveryPickUp;
    [SerializeField] Transform enemyPlayer;
    [SerializeField] Transform[] deliveryDropOff;
    [SerializeField] bool pickedUp;
    [SerializeField] int destinationIndex;
    NavMeshAgent navigationAgent;
    [SerializeField] driverStates currDriverTask;
    [SerializeField] float timer;
    public GameObject otherPlayer;

    void Start()
    {
        navigationAgent = GetComponent<NavMeshAgent>();
        currDriverTask = driverStates.Cruising;
        pickedUp = false;
        timer = 5.0f;
    }
    #endregion
   
    public driverStates getAction
    {
        get { return this.currDriverTask; }
    }
    public bool hasPickedUp
    {
        get { return pickedUp; }
    }

    public Transform carPosition
    {
        get { return this.transform; }
    }

    public void setEnemyPos(Transform trans)
    {
        enemyPlayer = trans;
    }
    void Update()
    {
        //Look at the top to see what conditions mean.
        switch(currDriverTask)
        {
            case driverStates.Cruising:
                #region
                //If there are no deliveries kicking about - keep in this state.
                #region general pathfinding method
                //if the path is no longer being calculated and the destination has been reached.
                if (!navigationAgent.pathPending && navigationAgent.remainingDistance < 1.0f)
                {
                    //calculate the next location to go to.
                    nextDestination(false);
                }
                #endregion
                //If a delivery is available - go to seeking delivery.
                #region transitonal condition
                if (deliveryPickUp[0] != null) //get delivery location if it exists
                {
                    nextDestination(true);
                    currDriverTask = driverStates.SeekingDelivery;
                }
                #endregion
                //adjust rotation to face the direction of movement
                transform.rotation = Quaternion.LookRotation(navigationAgent.velocity);
                #endregion
                break;
            case driverStates.SeekingDelivery:
                #region
                //Currently looking going to the delivery
                #region general pathfinding method
                //if the path is no longer being calculated and the destination has been reached.
                if (!navigationAgent.pathPending && navigationAgent.remainingDistance < 1.0f)
                {
                    //calculate the next location to go to.
                    pickedUp = true;
                    nextDestination(true);
                    currDriverTask = driverStates.AttemptingToDeliver;
                }
                #endregion
                //If i have the delivery - go to attempting to deliver.

                //If an enemy has the delivery - go to seeking enemy.

                //adjust rotation to face the direction of movement
                transform.rotation = Quaternion.LookRotation(navigationAgent.velocity);
                #endregion
                break;
            case driverStates.SeekingEnemy:
                getEnemyPos();
                #region
                if (!navigationAgent.pathPending && navigationAgent.remainingDistance < 1.0f)
                {
                    currDriverTask = driverStates.Cruising;
                }
                    //If the enemy has delivered the object - go to cruising.

                    //If i can see the player and are within range - go to attacking.
                    #endregion
                    break;
            case driverStates.AttemptingToDeliver:
                #region
                //If i have ran out of time - go to dumpfood.
                //If i have delivered food - go to delivered.
                #region reset to crusing
                if (!navigationAgent.pathPending && navigationAgent.remainingDistance < 1.0f)
                {
                    deliveryPickUp[0] = null;
                    deliveryDropOff[0] = null;
                    pickedUp = false;
                    currDriverTask = driverStates.Delivered;
                }
                #endregion
                //If a player is attacking me - go to dodge.
                //adjust rotation to face the direction of movement
                transform.rotation = Quaternion.LookRotation(navigationAgent.velocity);
                #endregion
                break;
            case driverStates.Delivered:
                #region
                //if there are pending deliveries - go to seek delivery.
                currDriverTask = driverStates.Cruising;
                //If there are no deliveries - go to crusing.
                #endregion
                break;
            case driverStates.Attacking:
                #region
                //If the attack is sucessful - go to attempting to deliver.
                //If the attack is unsucessful - go to seekingEnemy.
                //If the enemy has delivered the object - go to cruising.
                #endregion
                break;
            case driverStates.Dodging:
                #region
                //if I dodged sucessfully - go to attempting to deliver.
                //if i have been hit - go to dump food.
                #endregion
                break;
            case driverStates.Stunned:
                #region
                //pause for 3 secs.
                if (timer > 0)
                {
                    navigationAgent.Stop();
                    navigationAgent.ResetPath();
                    transform.Rotate(0, 360 * Time.deltaTime, 0);
                }
                else
                {
                    timer = 5.0f;
                    currDriverTask = driverStates.Cruising;
                }
                timer -= Time.deltaTime;
                //go to cruising.
                #endregion
                break;
            case driverStates.DumpFood:
                #region
                //If i was hit - go to stun.
                //else go to crusing.
                #endregion
                break;

        }
    }
    private void getEnemyPos()
    {
        navigationAgent.destination = enemyPlayer.position;
    }
    #region function calculate the next destination
    private void nextDestination(bool m_deliver)
    {

        if (m_deliver == false)
        {
            if (destinations.Length == 0)
            {
                return;
            }
            navigationAgent.destination = destinations[destinationIndex].position;
            destinationIndex = (destinationIndex + 1) % (destinations.Length);
        }
        else
        {
            if (pickedUp == false)
            {
                navigationAgent.destination = deliveryPickUp[0].position;
            }
            else
            {
                navigationAgent.destination = deliveryDropOff[0].position;
            }
        }
    }
    #endregion
}
