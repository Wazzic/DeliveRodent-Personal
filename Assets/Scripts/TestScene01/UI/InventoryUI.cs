using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    ////PRIVATE
    ////GameObject Which Stores the sliders
    //private GameObject inventory;
    ////Sliders for the item timer
    //private Slider[] playerItemTimer;
    ////Delivery script of player
    //private Delivery delivery;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //Gets the game obect that parents the sliders in the editor
    //    inventory = GameObject.Find("Inventory");
    //    //sets up the array to store the sliders
    //    playerItemTimer = new Slider[3];
    //    for (int i = 0; i < 3; i++)
    //    {
    //        //Gets the slider from the inventory object
    //        playerItemTimer[i] = inventory.transform.GetChild(i).GameObject().GetComponent<Slider>();
    //        //sets the maximum value for the slider
    //        playerItemTimer[i].maxValue = 10f;
    //    }
    //    //Gets the delivery script from the player
    //    delivery = GameObject.Find("Player0").GetComponent<Delivery>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (delivery)
    //    {
    //        //Updates the slider value
    //        for (int i = 0; i < 3; i++)
    //        {
    //            if (delivery.Items[i].Active)
    //            {
    //                playerItemTimer[i].value = delivery.Items[i].Timer;
    //            }
    //            else
    //            {
    //                playerItemTimer[i].value = 0;
    //            }
    //        }
    //    }
    //}

}
