using System.Collections.Generic;
using System.Collections;
using System.Threading;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//using Lofelt.NiceVibrations;

public class Delivery : MonoBehaviour
{
    //Structure for the Inventory
    [System.Serializable]
    public struct InventorySlot
    {
        public DeliveryItem item;
        public Slider UIElement;
        //public int meshIndex;
    }


    //PRIVATE
    //Storage for the delivery Manager
    private DeliveryManager deliveryManager;
    //Durations for the item decay
    [SerializeField]
    private float duration;
    //Representation of items for delivery
    private GameObject[] deliveryItems;
    private static ArrowScript[] arrowScripts;  //static might not work?

    //PUBLIC 
    //List of delivery items
    [SerializeField]
    public InventorySlot[] inventorySlots;

    [SerializeField] private GameObject markerPin;
    [SerializeField] private GameObject screenOutline;

    private DeliveryVFXHandler deliveryVFXHandler;
    private CarAudioHandler carAudioHandler;
    private CarVisualsHandler carVisualsHandler;
    private PlayerUIHandler playerUIHandler;
    //private RumbleController rumbleController;
    //private PlayerItemsHandler itemsHandler;
    
    public PromptHandler promptHandler;

    int numberOfFrames;
    [SerializeField][Range(50, 1000)] int delay;

    public SwitchMesh FoodMeshSwitch;

    [SerializeField]
    private int playerCameraLayer;
    [SerializeField]
    private bool TestDropOffLayer = true;

    private bool inventoryEmpty;

    //List<Billboard> billboards = new List<Billboard>();

    [SerializeField] public Material PlayerIcon;

    // Start is called before the first frame update
    void Start()
    {
        //Find the delivery manager script
        deliveryManager = GameObject.Find("SceneManager").GetComponent<DeliveryManager>();

        deliveryVFXHandler = GetComponentInChildren<DeliveryVFXHandler>();
        carAudioHandler = GetComponentInChildren<CarAudioHandler>();
        carVisualsHandler = GetComponentInChildren<CarVisualsHandler>();
        //rumbleController = GetComponent<RumbleController>();
        //itemsHandler = GetComponent<PlayerItemsHandler>();

        deliveryItems = carVisualsHandler.DeliveryItems;
        FoodMeshSwitch = SwitchMesh.instance;
        inventorySlots = new InventorySlot[3];

        playerUIHandler = GetComponentInChildren<PlayerUIHandler>();
        promptHandler = GetComponentInChildren<PromptHandler>();

        arrowScripts = FindObjectsOfType<ArrowScript>(); 

        for (int i = 0; i < 3; i++)
        {
            inventorySlots[i].item = new DeliveryItem();
            inventorySlots[i].UIElement = playerUIHandler.foodSliders[i];

            deliveryItems[i].SetActive(false);
            
        }

        markerPin.SetActive(false);
        screenOutline.SetActive(false);
    }

    //Updates delivery script
    private void Update()
    {
        //Updates items in inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item.Active)
            {
                //Reduces the time in current item timer
                inventorySlots[i].item.Timer -= Time.deltaTime;
                if (inventorySlots[i].item.Timer < 30)
                {
                    if (numberOfFrames > delay)
                    {
                        //deliveryVFXHandler.NudgeDeliveryScaleSpring(i);
                        playerUIHandler.NudgeSpring(i);
                        numberOfFrames = 0;
                    }
                    else
                    {
                        numberOfFrames++;
                    }
                }
                //When timer is less than 0 delivery has failed
                if (inventorySlots[i].item.Timer <= 0)
                {
                    OrderFailedDelivery(i);
                }

            }
            //Updates Slider Element
            if (inventorySlots[i].UIElement)
            {
                inventorySlots[i].UIElement.value = inventorySlots[i].item.Timer;
            }
        }
    }

    //Checks to see if the player has a delivery Note: DONT USE THIS EVERY FRAME
    public bool HasAnyDeliveries()
    {
        //Inventory empty is used the opposite way around here, probs need to change to be better name
        inventoryEmpty = false;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item.Active)
            {
                //Debug.Log("has active");
                inventoryEmpty = true;
                deliveryVFXHandler.SmellTrail.EnableTrail();
                markerPin.SetActive(true);
                screenOutline.SetActive(true);
            }
        }
        if (!inventoryEmpty)
        {
            markerPin.SetActive(false);
            screenOutline.SetActive(false);
            deliveryVFXHandler.SmellTrail.DisableTrail();
        }

        //carVisualsHandler.BodyScaleSpring(new Vector3(8.0f, 0.5f, 8.0f));
        ScoreManager.instance.CheckAllPlayerRanks();
        return inventoryEmpty;
    }
    //Checks to see if there is space in the drivers inventory
    public bool IsThereSpace()
    {
        foreach (InventorySlot invetory in inventorySlots)
        {
            if (!invetory.item.Active)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsInventoryEmpty()
    {
        return inventoryEmpty;
    }


    //Resets the value for the tiem slot
    public void ResetItemSlot(int itemNum)
    {
        if (inventorySlots.Length > itemNum)
        {
            inventorySlots[itemNum].item = new DeliveryItem();
            inventorySlots[itemNum].item.Active = false;
            inventorySlots[itemNum].item.Timer = -1;
        }
    }
    //Resets the value of the item slot and removes the item from the world list
    public void RemoveItemFromWorld(int itemNum)
    {
        deliveryManager.RemoveItemFromList(inventorySlots[itemNum].item);
        ResetItemSlot(itemNum);
    }

    //When a delivery has failed it does some functionality
    public void OrderFailedDelivery(int itemNum)
    {
        deliveryVFXHandler.deliverySprings[itemNum].DeliveryRanOut();
        //Disables the current drop off point
        deliveryManager.DisableDropOffPoint(inventorySlots[itemNum].item.DeliveryLocation.GetComponent<DropOff>().DropOffID);
        //Removes the item from the world item list
        RemoveItemFromWorld(itemNum);
        //Activates the next pick up point
        if (!DeliveryManager.instance.spawningPoints)
        {
            //deliveryManager.RemoveItemFromList()
            //RemoveItemFromWorld(itemNum);
            deliveryManager.OvertimeRemoveActiveAmount();
        }
        else if (DeliveryManager.instance.spawningPoints)
        {
            deliveryManager.ActivatePickUp();
        }

        //deliveryVFXHandler.NudgeDeliveryScaleSpring(itemNum);
        //checks if the player has any items in there inventory
        HasAnyDeliveries();

        //disables the item model visual
        //deliveryItems[itemNum].SetActive(false);        
        //Hides the timer ui element
        playerUIHandler.SetTimerVisible(itemNum, 0.0f);

        //itemsHandler.PlayerRank();
    }

    //Stores the item for the delivery
    public void PickUpOrder(DeliveryItem item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //Checks for avaiable slot
            if (!inventorySlots[i].item.Active)
            {
                GetComponent<PlayerScore>().AddScore(1f);
                
                ScoreManager.instance.CheckAllPlayerRanks();

                //set item values 
                inventorySlots[i].item = item;
                inventorySlots[i].item.Active = true;
                inventorySlots[i].item.transform = deliveryItems[i].transform;
                inventorySlots[i].item.Owner = gameObject;

                SwitchDropOffLayer(item);

                //Hangles the setup for the timer visual
                if (inventorySlots[i].UIElement)
                {
                    inventorySlots[i].UIElement.maxValue = item.TimeForDelivery;
                    playerUIHandler.SetTimerVisible(i, 1);
                    Image tempImage = inventorySlots[i].UIElement.transform.GetChild(1).GetComponent<Image>();
                    Color tempColor = new Color(item.mat.color.r, item.mat.color.g, item.mat.color.b, 1.0f);
                    tempImage.color = tempColor;

                    //change the colour of the delivery billboard to same as item.mat.color (i.e tempColor)
                    //billboards[i].GetComponent<MeshRenderer>().material.color = tempColor;
                }
                //Changes the mesh of the food from one of the meshes in the FoodMeshSwitch storage
                if (FoodMeshSwitch)
                {
                    deliveryItems[i].SetActive(true);
                    inventorySlots[i].item.meshIndex = FoodMeshSwitch.ChangeRandomMesh(deliveryItems[i].GetComponent<MeshFilter>(), deliveryItems[i].GetComponent<MeshRenderer>());

                }
                //Plays Visual effect
                deliveryVFXHandler.PlayPickUpVFX();
                deliveryVFXHandler.NudgeDeliveryScaleSpring(i);

                //Plays Haptics                
                //rumbleController.PlayRumble(0.35f, 0.5f, 0.5f);

                HasAnyDeliveries();
                //Applyies a change to the player sprite on order pickup
                //gameObject.GetComponent<PlayerStatus>().OrderPickUp();
                //Plays sound
                carAudioHandler.PlayPickUpAudio();

                //Checks all the players arrows
                //ArrowScript[] arrowScripts = FindObjectsOfType<ArrowScript>();  //CHANGE THIS!!
                for (int j = 0; j < arrowScripts.Length; j++)
                {
                    if (arrowScripts[j].transform.root != gameObject.transform)
                    {
                        //Activate chase delivery prompt here!!
                        if (!PromptManager.instance.promptHandlers[j].hasStolen)
                        {
                            //THIS - PromptManager.instance.promptHandlers[j].hasStolen = true;
                            PromptManager.instance.promptHandlers[j].ShowPromptindex(2);
                                //Show correct index (chase delivery)
                        }

                        //Checks if any of the players are pointing to the item picked up
                        if (arrowScripts[j].GetCurrentItem() == item)
                        {
                            //Visualises the players anger/comfusion at the order they were aiming for being picked up
                            arrowScripts[j].PlayComfusedEmote();
                        }
                    }
                    else
                    //if is the player who picked up
                    {
                        if (!PromptManager.instance.promptHandlers[j].hasDropedOff)
                            //This means the prompt is only shown to once. hasDropped is set in DropOff.
                        {
                            //THIS - PromptManager.instance.promptHandlers[j].hasPickedUp = true;
                            PromptManager.instance.promptHandlers[j].ShowPromptindex(1);
                                //Show drop off prompt
                        }
                    }
                }
                break;
            }
        }
    }

    //When players gets damaged call script
    public List<DeliveryItem> PlayerDamaged()
    {

        List<DeliveryItem> m_list = new List<DeliveryItem>();
        //if(list.Count == 0)
        // {
        //     return null;
        // }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //checks for valid item
            if (inventorySlots[i].item.Active)
            {
                m_list.Add(inventorySlots[i].item);
                ResetItemSlot(i);
                playerUIHandler.SetTimerVisible(i, 0);
                deliveryItems[i].SetActive(false);
                deliveryVFXHandler.PlayStolenVFX();
            }
            else
            {
                //Play effect without any smoke
            }
        }
        //deliveryVFXHandler.particleEffects[1].Play();
        HasAnyDeliveries();
        return m_list;
    }

    //When player stealing items from other player add items to array
    public void RecieveItemFromPlayer(List<DeliveryItem> list)
    {
        //Checks for valid list
        if (list == null)
        {
            return;
        }
        //Checks to see if list has stored info
        if (list.Count == 0)
        {
            return;
        }

        //Add items to player
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //Checks to see if tere is still info
            if (list.Count == 0)
            {
                return;
            }
            //Adds item to the player
            if (!inventorySlots[i].item.Active)
            {
                //Gets the item at the back of the list and stores it in the inventory space
                DeliveryItem currentItem = list[list.Count - 1];
                currentItem.Stolen();
                SwitchDropOffLayer(currentItem);
                inventorySlots[i].item = currentItem;
                //Updates the item transform to be set to the current player
                inventorySlots[i].item.transform = deliveryItems[i].transform;
                inventorySlots[i].item.Owner = gameObject;
                //Enables the timer
                playerUIHandler.SetTimerVisible(i, 1f);
                inventorySlots[i].UIElement.maxValue = inventorySlots[i].item.TimeForDelivery;

                //Gets the image for the timer
                Image tempImage = inventorySlots[i].UIElement.transform.GetChild(1).GetComponent<Image>();
                //Gets the item colour and stores the alpha channel as 1 unit
                Color tempColor = new Color(currentItem.mat.color.r, currentItem.mat.color.g, currentItem.mat.color.b, 1.0f);
                tempImage.color = tempColor;
                //billboards[i].GetComponent<MeshRenderer>().material.color = tempColor;

                //Updates and Ativates the mesh model
                FoodMeshSwitch.ChangeMesh(currentItem.meshIndex, deliveryItems[i].GetComponent<MeshFilter>(), deliveryItems[i].GetComponent<MeshRenderer>());
                deliveryItems[i].SetActive(true);

                //Applys effect
                deliveryVFXHandler.NudgeDeliveryScaleSpring(i);
                //Plays Sound
                //carAudioHandler.PlayStealAudio();

                //Removes item from list
                list.RemoveAt(list.Count - 1);
            }
        }
        //Remove any spare items in play
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                //Remove the item from the world
                deliveryManager.RemoveItemFromList(list[i]);
                //Disables the current drop off point
                deliveryManager.DisableDropOffPoint(list[i].DeliveryLocation.GetComponent<DropOff>().DropOffID);
                //Activates the next pick up point
                deliveryManager.ActivatePickUp();
            }
        }
        HasAnyDeliveries();
    }

    //Removes the item from the inventory when delivered
    public void DropOffOrder(DeliveryItem item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //Checks to see if the id mataches
            if (inventorySlots[i].item == item && inventorySlots[i].item.Active == true)
            {
                //ArrowScript[] arrowScripts = FindObjectsOfType<ArrowScript>();
                for (int j = 0; j < arrowScripts.Length; j++)
                {
                    if (arrowScripts[j].transform.root != gameObject.transform)
                    {
                        
                        if (arrowScripts[j].GetCurrentItem() == item)
                        {
                            arrowScripts[j].PlayAngryEmote();
                        }
                    }
                    //Activate pick up delivery prompt here!
                    if (!PromptManager.instance.promptHandlers[j].hasPickedUp)
                    {
                        //PromptManager.instance.promptHandlers[j].hasDropedOff = true;
                        PromptManager.instance.promptHandlers[j].ShowPromptindex(0);
                        //Show Arrow to pick up prompt
                    }
                }

                deliveryItems[i].SetActive(false);
                //resets values for inventory slot
                DeliveryManager.instance.RemoveItemFromList(inventorySlots[i].item);
                playerUIHandler.SetTimerVisible(i, 0.0f);

                ResetItemSlot(i);

                deliveryVFXHandler.PlayDropOffVFX();
                carAudioHandler.PlayDropOffAudio();
                                
                //rumbleController.PlayRumble(0.35f, 0.5f, 0.5f);
                //ScoreManager.instance.UpdateText();
            }
        }
        HasAnyDeliveries();
    }

    //Checks for correct order
    public bool CheckCorrectOrder(DeliveryItem item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //check if id matches
            if (inventorySlots[i].item == item && inventorySlots[i].item.Active == true)
            {
                return true;
            }
        }
        return false;
    }
    public bool PlayerHasItem(DeliveryItem item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (item == inventorySlots[i].item)
            {
                return true;
            }
        }

        return false;
    }

    public bool PlayerHasItem()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.item.Active)
            {
                return true;
            }
        }

        return false;
    }

    //Sets the Drop Off point for the item to be only seen by the current owner of the item
    private void SwitchDropOffLayer(DeliveryItem item)
    {
        if (TestDropOffLayer)
        {
            item.DeliveryLocation.layer = playerCameraLayer;
            item.DeliveryLocation.transform.GetChild(0).gameObject.layer = playerCameraLayer;
        }
    }
}