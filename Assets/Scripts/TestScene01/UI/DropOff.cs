using System;
using System.Collections;
using UnityEngine;

public class DropOff : MonoBehaviour
{
    //PRIVATE
    //Stores copy of delivery manager
    private DeliveryManager deliveryManager;
    [SerializeField]
    private PedestrianScript pedestrian;
    //PUBLIC
    //Drop off id is point in array
    public DeliveryItem ItemToBeDelivered;

    public GameObject foodVisual;

  

    [SerializeField]
    private ParticleSystem smokeScreen;
    [SerializeField]
    private Transform pedestrianLocation;

    [SerializeField]
    private bool StopWhenDropOff = false;

    private bool followItem = true;

    [SerializeField]
    private GameObject[] visualAssets;

    public int DropOffID;
    private void Start()
    {
        followItem = true;
        deliveryManager = GameObject.Find("SceneManager").GetComponent<DeliveryManager>();
    }

    private void Update()
    {
        if(ItemToBeDelivered!= null && followItem) pedestrian.PedestrianLookAtItem(ItemToBeDelivered.transform);
    }

    //Checks for trigger collision
    private void OnTriggerEnter(Collider other)
    {
        
        //Gets delivery component
        Delivery m_delivery = other.GetComponent<Delivery>();
        if (!m_delivery)
        {
            //Debug.Log("No Delivery Script Attached");
            return;
        }
        //Gets character score component
        PlayerScore m_score = other.GetComponent<PlayerScore>();
        if (!m_score)
        {
            //Debug.Log("No Score Script Attached");
            return;
        }
        //Checks to see if the delivery driver has the order
        if (m_delivery.CheckCorrectOrder(ItemToBeDelivered))
        {        
            //Adds score to the deliver drier
            m_score.AddScore(ItemToBeDelivered.TotalPayment());

            //NEEDS TO BE IMPROVED TEMPORARY IMPLEMENTATION
            SwitchMesh.instance.ChangeMesh(ItemToBeDelivered.meshIndex, foodVisual.GetComponent<MeshFilter>(), foodVisual.GetComponent<MeshRenderer>());
            foodVisual.transform.position = ItemToBeDelivered.transform.position;          

            StartCoroutine(AnimateFoodDelivery());

            //Drops off the order for the delivery, aka: removes the item from the delivery driver
            m_delivery.DropOffOrder(ItemToBeDelivered);

            //Spawns the next pick up location
            if (deliveryManager.spawningPoints)
            {
                //m_delivery.RemoveItemFromWorld(Array.IndexOf(m_delivery.inventorySlots, ItemToBeDelivered));
                deliveryManager.ActivatePickUp();
            }
            else
            {
                deliveryManager.OvertimeRemoveActiveAmount();
            }

            //AnalyticsManager.instance.successfulDeliveries++;
            //AnalyticsManager.instance.DeliveryCompletedEvent(ItemToBeDelivered.Timer, ItemToBeDelivered.TimeForDelivery);

            int promptIndex = PromptManager.instance.promptHandlers.IndexOf(m_delivery.promptHandler);
            //THIS - PromptManager.instance.promptHandlers[promptIndex].hasDropedOff = true;

           
            // Slow the player down
            if(StopWhenDropOff) other.GetComponent<ArcadeVehicleController>().SlowToStopFunction();
            //Shows the first delivery completed prompt to all players
            if (!PromptManager.instance.firstDelivery)
            {
                //commented out because the player who picks up will see 2 prompts which is a lot
                //PromptManager.instance.ShowPromptToAll(2); 
                PromptManager.instance.firstDelivery = true;
            }


           
            //Disables the drop off object
            //Invoke("DisableGameObject", 1.5f);

        }
    }

    private IEnumerator AnimateFoodDelivery()
    {
        followItem = false;
        foreach(GameObject visual in visualAssets)
        {
            visual.SetActive(false);
        }

        float time = 0.0f;
        Vector3 startPos = foodVisual.transform.position;

        //Total distance to used to normalize the distance
        float totalDistance = Vector2.Distance(new Vector2(startPos.x, startPos.z), new Vector2(pedestrianLocation.position.x, pedestrianLocation.position.z));


        while (time < 1.25f)
        {
            time += Time.deltaTime;
            //Normalized time
            float normalizedTime = (time - 0) / (1.25f - 0);
            //Calculates the current distance of the food
            float distance = Vector2.Distance(new Vector2(foodVisual.transform.position.x, foodVisual.transform.position.z), new Vector2(pedestrianLocation.position.x, pedestrianLocation.position.z));
            
            //Used to calculate a height based off the distance
            float normalizedheight = ((totalDistance - distance) / totalDistance) * Mathf.PI;
            normalizedheight = MathF.Sin(normalizedheight);
            //Moves the food from the start pos to the pedestrian
            foodVisual.transform.position = Vector3.Lerp(startPos, pedestrianLocation.position, normalizedTime);
            //Applies a height to the position so that it moves in an arc shape
            foodVisual.transform.position = foodVisual.transform.position + new Vector3(0, normalizedheight * 7.5f, 0);
            //Applies a rotation to the object
            foodVisual.transform.Rotate(new Vector3(2, 2, 0));
            
            yield return null;
        }

        //Hides the mesh for the food
        foodVisual.GetComponent<MeshRenderer>().enabled = false;
        //Starts the particle effect for the smokescreen
        smokeScreen.Stop();
        smokeScreen.time = 0;
        while(!smokeScreen.isPlaying)
        {
            smokeScreen.Play();
        }
        yield return new WaitForSeconds(0.5f);
        pedestrian.Character.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        DeactivateZone();
        //float t = 0f;
        //while (smokeScreen.time < 0.5f)
        //{
        //    Debug.LogError("WaitingPlayerDisapear");
        //    yield return null;
        //}
        ////Deactivates the pedestrian character so that they are hidden
        ////pedestrian.Character.gameObject.SetActive(false);
       
        //yield return null;

        ////Waits for the animation to end
        //while (smokeScreen.time < 1f)
        //{
        //    Debug.LogError("WaitingSmokeScreenToEnd");
        //    yield return null;
        //}
        ////Deactivates the zone to be used again
        //    Debug.LogError("SUCSES");
        //DeactivateZone(1);
        //yield return null;

    }


    private void DeactivateZone()
    {
        //Renalbe deactivated objects for next zone activation
        pedestrian.Character.gameObject.SetActive(true);
        foodVisual.GetComponent<MeshRenderer>().enabled = true;
        foreach (GameObject visual in visualAssets)
        {
            visual.SetActive(true);
        }

        //Sets the drop off to inactive
        deliveryManager.DisableDropOffPoint(DropOffID);
        //Remove item from zone
        ItemToBeDelivered = null;

        followItem = true;

        foodVisual.GetComponent<MeshFilter>().mesh = null;
    }
}