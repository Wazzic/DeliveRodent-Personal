using UnityEngine;
using Spring;
using Spring.Runtime;

public class PickUp : MonoBehaviour
{
    //PRIVATE
    //Storage for the delivery driver
    private DeliveryManager deliveryManager;
    
    //PUBLIC
    //Pick up id is point in array
    public int PickUpID;

    private DeliveryItem deliveryItem;

    public void SetDeliveryItem(DeliveryItem item)
    {
        deliveryItem = item;
    }

    public DeliveryItem GetDeliveryItem()
    {
        return deliveryItem;
    }

    //On start up
    private void Start()
    {        
        deliveryManager = GameObject.Find("SceneManager").GetComponent<DeliveryManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //gets the delivery script from the driver
        Delivery m_delivery = other.GetComponent<Delivery>();
        if (!m_delivery)
        {
            return;
        }
        //Checks to see if driver has space
        if (!m_delivery.IsThereSpace())
        {
            return;
        }
        //Stores the item for the driver
        m_delivery.PickUpOrder(deliveryManager.ActivateDropOff(deliveryItem));
        //disables the pick up object
        deliveryManager.DisablePickUpPoint(PickUpID);               
    }
}