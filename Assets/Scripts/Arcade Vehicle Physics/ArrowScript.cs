using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Spring;
using Spring.Runtime;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;

public class ArrowScript : MonoBehaviour
{
    SpringToScale scaleSpring;
    SpringToRotation rotationSpring;

    //[SerializeField] DeliveryVFXHandler vFXHandler;
    
    Transform myCar;
    DeliveryManager deliveryManager;
    [SerializeField] TargetIconHandler targetIconHandler;
    [SerializeField] Delivery delivery;
    [SerializeField] TextMeshProUGUI distanceText;
    //  [SerializeField] Material dropOffMat;
    //  [SerializeField] Material pickUpMat;
    [SerializeField] MeshRenderer[] listMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;

    [SerializeField] Camera playerCamera;
    

    private bool lookAtDropOff;
    private int currentTarget;

    private bool switchTarget;
    private bool switchTargetType;
    private bool switchTargetCheckInput;

    [SerializeField] private DeliveryVFXHandler vFXHandler;

    private DeliveryItem currentItem;
    private GameObject currentOwner;

    Vector3 offset;

    [SerializeField] int distance;

   

    public void OnSwitchArrowTargetButton(InputAction.CallbackContext context)
    {
        switchTarget = context.action.WasPressedThisFrame();
        scaleSpring.Nudge(new Vector3(6, 2, 6));
    }

    public void OnSwitchArrowTypeButton(InputAction.CallbackContext context)
    {
        switchTargetType = context.action.WasPressedThisFrame();
        //scaleSpring.Nudge(new Vector3(5, 2, 5));
    }
    private void Awake()
    {
        rotationSpring = GetComponent<SpringToRotation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        deliveryManager = DeliveryManager.instance;
        myCar = transform.root;

        delivery = transform.root.GetComponent<Delivery>();
        listMaterial = GetComponentsInChildren<MeshRenderer>();

        scaleSpring = GetComponent<SpringToScale>();

        offset = transform.localPosition;
        offset.x = 0;
        offset.z = 0;
        //this.transform.parent = null;
    }
    void LateUpdate() // late update to avoid jittering
    {
        if(DeliveryManager.instance.ActiveAmount == 1)
        {
            SingleDelivery();
        }
        else
        {
            MultipleDelivery();
        }

    }

    private void MultipleDelivery()
    {

        //Switches Current Target
        if (switchTarget)
        {
            currentTarget++;
            switchTarget = false;
        }

        if(deliveryManager.GetItemsInWorld().Count == 0)
        {
            return;
        }

        //Avoids the current target from being out of range
        if (deliveryManager.GetItemsInWorld().Count > 0 && currentTarget > deliveryManager.GetItemsInWorld().Count - 1)
        {
            currentTarget = 0;

        }
        else if (currentTarget < 0)
        {
            currentTarget = deliveryManager.GetItemsInWorld().Count - 1;
        }

        currentItem = deliveryManager.GetItemsInWorld()[currentTarget];

        //Checks to see if the current target exists
        if (currentItem == null)
        {
            currentTarget++;
            return;
        }
        //Updates the arrow and icon colur
        UpdateArrowIconColour(currentItem.mat.color);
        //Updates the rotation of the arrow
        UpdateArrowRotation(currentItem);
    }

    public void SingleDelivery()
    {

        if (deliveryManager.GetItemsInWorld().Count > 0)
        {
            if (deliveryManager.GetItemsInWorld()[0] == null) return;

            if (currentItem != null)
            {
                if (currentItem.Owner != currentOwner)
                {
                    scaleSpring.Nudge(new Vector3(18, 6, 18));
                    currentOwner = currentItem.Owner;

                    if (delivery.PlayerHasItem(currentItem))
                    {
                        playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("SingleDeliveryHasItem");
                        playerCamera.cullingMask &= ~ (1 << LayerMask.NameToLayer("SingleDeliveryNoItem"));
                    }
                    else
                    {
                        playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("SingleDeliveryNoItem");
                        playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SingleDeliveryHasItem"));
                    }

                }
            }
          

            //Gets the current item in the world
            currentItem = deliveryManager.GetItemsInWorld()[0];
            

            //Check to see if it is a valid item
            if (currentItem == null) return;
            

            //Updates the rotation of the arrow
            UpdateArrowRotation(currentItem);

            //Changes the colour of the arrow dependent on if the arrow has been picked up by a player or not
            if (deliveryManager.ActivePickUpPoints.Count > 0)
            {
                UpdateArrowIconColour(deliveryManager.zoneMaterials.defaultPickUpMat.color);
            }
            else
            {
                //Checks whether the arrow is pointing to a player or a drop off
                if (delivery.PlayerHasItem(currentItem))
                {
                    UpdateArrowIconColour(deliveryManager.zoneMaterials.defaultDropOffMat.color);
                }
                else
                {
                    UpdateArrowIconColour(deliveryManager.zoneMaterials.defaultPlayerMat.color);
                }
              
            }

         

        }
    }

    private void UpdateArrowIconColour(Color colour)
    {
        //Change Colour of arrow and icon
        for (int i = 0; i < listMaterial.Length; i++)
        {
            //listMaterial[i].material.color = colour;
            listMaterial[i].sharedMaterial.color = new Color(colour.r, colour.g, colour.b, 1f);
            listMaterial[i].sharedMaterial.SetColor("_EmissionColor", colour);
            targetIconHandler.ChangeColour(colour);
       }
    }

    //Changes the arrow rotation based off of the current ite,
    private void UpdateArrowRotation(DeliveryItem currentItem)
    {
        //Checks if the player has the current item
        if (delivery.PlayerHasItem(currentItem))
        {
            //Updates the rotation of the arrow to point to the drop off zone
            transform.forward = UpdateLookAtTarget(currentItem.DeliveryLocation.transform.position);
            float dot = Vector3.Dot(transform.forward, myCar.transform.forward);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + dot * 10, transform.localEulerAngles.y, transform.localEulerAngles.z);
            //Changes icon to represent the drop off zone
            targetIconHandler.ChangeIcon(1);

        }
        else
        {
            //Updates the rotation of the arrow to point to where the item is in the world
            transform.forward = UpdateLookAtTarget(currentItem.transform.position);
            float dot = Vector3.Dot(transform.forward, myCar.transform.forward);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + dot * 10, transform.localEulerAngles.y, transform.localEulerAngles.z);

            //Changes the icon if either a player has the item or yet to be picked up
            if (currentItem.Owner.GetComponent<Delivery>())
            {
                targetIconHandler.ChangeIcon(currentItem.Owner.GetComponent<Delivery>().PlayerIcon);

            }
            else
            {
                targetIconHandler.ChangeIcon(0);

            }

        }
    }


    public DeliveryItem GetCurrentItem()
    {
        return currentItem;
    }

    public void PlayAngryEmote()
    {
        vFXHandler.PlayAngryEmoji();
    }

    public void PlayComfusedEmote()
    {
        vFXHandler.PlaySurprisedEmoji();
    }


    public Vector3 UpdateLookAtTarget(Vector3 newLookAtTarget)
    {
        newLookAtTarget.y = myCar.transform.position.y;
        Vector3 forward = (newLookAtTarget - myCar.transform.position);

        distance = ((int)forward.magnitude);
        if (distanceText)
        {
            string t = distance.ToString() + "m";
            distanceText.text = t;
        }
        return forward.normalized;
    }
}
