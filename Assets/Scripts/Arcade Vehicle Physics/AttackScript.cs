using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class AttackScript : MonoBehaviour
{
    //PUBLIC
    //PRIVATE
    //delivery VFX Handler
    private DeliveryVFXHandler deliveryVFXHandler;
    //ID
    private int ID;
    //Players delivery script
    private Delivery myObjectDelivery;
    //Attack Collider
    private SphereCollider attackCollider;
    [SerializeField] private float attackRadiusBig; // 20 is what it was before having big and small radius
    [SerializeField] private float attackRadiusSmall; 
    [SerializeField] private float stunDurationSmall; 
    [SerializeField] private float stunDurationBig; 
    [SerializeField] private Transform attackVFX; 
    [SerializeField] float vfxScaleFactor = 1f;
    [SerializeField] LayerMask layerMask;
    //Player status script
    private PlayerStatus playerStatus;
    private ArcadeVehicleController arcadeVehicleController;
    private CarAudioHandler carAudioHandler;
    //Cooldown for attack
    private float lastAttacked = float.MinValue;
    private float attackCooldown;
    //Input bool for attack
    private bool playerInTrigger;
    private bool attackButton;
    //[SerializeField] GameObject stealButtonPrompt;
    [SerializeField] Prompt attackButtonPrompt;

    public void OnStealButton(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && InputManager.instance.controlsEnabled)
        {
            //deliveryVFXHandler.PlayAttackVFX();
            attackButton = true;
            
        }
        else
        {
            attackButton = false;
        }
    }
    private void Awake()
    {
        carAudioHandler = transform.root.GetComponentInChildren<CarAudioHandler>();
        playerStatus = transform.parent.AddComponent<PlayerStatus>();
        arcadeVehicleController = transform.root.GetComponent<ArcadeVehicleController>();
    }
    private void Start()
    {
        //VFX For deliveries
        deliveryVFXHandler = transform.parent.GetComponentInChildren<DeliveryVFXHandler>();
        ID = GetComponentInParent<PlayerLocalManager>().ID;
        myObjectDelivery = GetComponentInParent<Delivery>();
        attackCollider = GetComponent<SphereCollider>();
        attackCollider.radius = attackRadiusBig;
        //stealButtonPrompt.SetActive(false);
        //attackButtonPrompt.
        attackCooldown = 1.25f;
    }
    // Fixed update runs before trigger stay and update
    private void FixedUpdate()
    {
        playerInTrigger = false;
    }

    private void Update()
    {
        // Set the active state of the stealButtonPrompt GameObject based on the value of the "canAttack" variable.
        // If "canAttack" is true, set the GameObject to active; otherwise, set it to inactive.
        //stealButtonPrompt.SetActive(playerInTrigger ? true : false);
        if (playerInTrigger)
        {
            attackButtonPrompt.ShowPromptSpringTo();
        }
        else
        {
            attackButtonPrompt.HidePrompt();
        }


        SetAttackColliderRadius();

        //Checks to see if the player attack is avaiable and if the player isnt stunned
        if (attackButton && IsCooldownFinished() && !arcadeVehicleController.IsStunned)
        {
            attackButton = false;
            //Attacks arouns the player
            Attack();
            
        }
    }
    void SetAttackColliderRadius()
    {        
        if (myObjectDelivery.IsInventoryEmpty())
        {
            attackVFX.localScale = new Vector3(attackRadiusSmall, attackRadiusSmall, attackRadiusSmall) * vfxScaleFactor;
            attackCollider.radius = attackRadiusSmall;
        }
        else
        {
            attackVFX.localScale = new Vector3(attackRadiusBig, attackRadiusBig, attackRadiusBig) * vfxScaleFactor;
            attackCollider.radius = attackRadiusBig;
        }
    }
    private bool IsCooldownFinished()
    {
        return lastAttacked + attackCooldown < Time.time;
    }

    void Attack()
    {
        Collider[] colliders = new Collider[4];
        int totalCollision = 0;
        totalCollision = Physics.OverlapSphereNonAlloc(transform.position, attackCollider.radius, colliders, layerMask);
        
        //Plays the attack visual effect && Haptic feedback
        deliveryVFXHandler.PlayAttackVFX();
        carAudioHandler.PlaySwoosh();
        //GamepadRumbler.SetCurrentGamepad(arcadeVehicleController.PlayerID);
        //HapticPatterns.PlayEmphasis(1f, 0.2f);

        bool successfulAttack = false;
        for (int i = 0; i < totalCollision; i++)
        {
            if (colliders[i].IsUnityNull())
            {
                continue;
            }
            if (colliders[i].GetComponent<Delivery>() != null && colliders[i].transform.root != transform.root) //check not colliding with self
            {
                //Gets the other players status script
                PlayerStatus otherPlayerStatus = colliders[i].GetComponent<PlayerStatus>();
                ///gets the othe players deliver script
                Delivery otherObjectDelivery = colliders[i].GetComponent<Delivery>();
                
                //Checks if the player is invincible
                if (!otherPlayerStatus.Invincible && !otherPlayerStatus.IsStopping)
                {
                    StunOtherPlayer(otherPlayerStatus, otherObjectDelivery);
                    //Gets the items from the attacked players
                    var otherItems = otherObjectDelivery.PlayerDamaged();
                    //Gives this player the items from the attacked player
                    myObjectDelivery.RecieveItemFromPlayer(otherItems);
                    //Stuns the attacked player
                    otherObjectDelivery.HasAnyDeliveries();
                    myObjectDelivery.HasAnyDeliveries();


                    //AnalyticsManager.instance.steals++;
                    successfulAttack = true;
                }
            }
        }
        if (successfulAttack)
        {
            return;
        }
        //Sets the time for last attack
        lastAttacked = Time.time;

    }

    private void StunOtherPlayer(PlayerStatus otherPlayerStatus, Delivery otherObjectDelivery)
    {
        if (otherObjectDelivery.HasAnyDeliveries())
        {
            carAudioHandler.PlayStealAudio();
            otherPlayerStatus.StunPlayerWithIFrames(stunDurationBig, AngleDir(otherObjectDelivery.transform.position));
        }
        else
        {
            carAudioHandler.PlayAttackNotSteal();
            otherPlayerStatus.StunPlayerWithIFrames(stunDurationSmall, AngleDir(otherObjectDelivery.transform.position));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsCooldownFinished() == false)
            return;
        // To show button prompt
        if (other.GetComponent<Delivery>() != null)
        {
            //Gets the other players status script
            PlayerStatus otherPlayerStatus = other.GetComponent<PlayerStatus>();
            //Checks if the player is invincible and we are not stunnerd
            if (!otherPlayerStatus.Invincible && !arcadeVehicleController.IsStunned && !otherPlayerStatus.IsStopping)
            {
                playerInTrigger = true;
            }
        }
    }


    private Vector3 AngleDir(Vector3 otherPlayerPos)
    {
        Vector3 delta = (otherPlayerPos - transform.position).normalized;

        // Calculate the perpendicular vector to determine the direction of the angle
        Vector3 perp = Vector3.Cross(transform.forward, delta);

        // Calculate the dot product between the perpendicular vector and the up direction
        float dir = Vector3.Dot(perp, Vector3.up);

        // Check the direction of the angle
        if (dir > 0f)
        {
            // The angle is in the positive direction
            return transform.right;
        }
        else if (dir < 0f)
        {
            // The angle is in the negative direction
            return -transform.right;
        }
        else
        {
            // The vectors are perpendicular, the angle is neither positive nor negative
            return perp * -1; // shuv it left anyways
        }
    }
}