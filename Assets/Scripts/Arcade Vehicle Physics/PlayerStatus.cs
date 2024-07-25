using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private float initalStun;
    private float stunnedLength;


    //Invincibilty
    public bool Invincible = false;
    public bool IsStopping = false;
    private float initalInvincibilty;
    private float invincibiltyLength = 3f;

    private enum SpriteStatus { 
        DeliverAquired = 0,
        DeliverStolen = 1,
        Neutral = 2
    }

    //Invincibilty
    private bool order = false;
    private float initalOrderTime;
    private float orderLength = 1.0f;

    [SerializeField]
    private Sprite[] SpriteImages;

    [SerializeField]
    private Image playerSpriteRenderer;

    ArcadeVehicleController arcadeVehicleController;
    [SerializeField] GameObject bubble;
    // Start is called before the first frame update
    void Start()
    {
        //playerSpriteRenderer = transform.Find("PlayerUI").Find("PlayerUI").Find("PlayerSprite").GetComponent<Image>();
        SpriteImages = new Sprite[3];
        //SpriteImages[(int)SpriteStatus.DeliverAquired] = Resources.Load<Sprite>("Sprites/RodentDeliverAquiredSprite");
        //SpriteImages[(int)SpriteStatus.DeliverStolen] = Resources.Load<Sprite>("Sprites/RodentDeliverStolen");
        //SpriteImages[(int)SpriteStatus.Neutral] = Resources.Load<Sprite>("Sprites/RodentDeliverNeutral");
        //playerSpriteRenderer.sprite = SpriteImages[(int)SpriteStatus.Neutral];
        arcadeVehicleController = GetComponent<ArcadeVehicleController>();
        bubble = transform.Find("Bubble").gameObject;
        bubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Stun Timer
        //if (StunnedForOneFrame)
        //{
        //    if(initalStun + stunnedLength < Time.time)
        //    {
        //        StunnedForOneFrame = false;
        //        //playerSpriteRenderer.sprite = SpriteImages[(int)SpriteStatus.Neutral];
        //    }
        //}
        //Invincible Timer
        if (Invincible)
        {
            if (initalInvincibilty + invincibiltyLength < Time.time)
            {
                bubble.SetActive(false);
                Invincible = false;
            }
        }
        //Order Racoon Visual Timer
        if (order)
        {
            if (initalOrderTime + orderLength < Time.time)
            {
                order = false;
                //playerSpriteRenderer.sprite = SpriteImages[(int)SpriteStatus.Neutral];
            }
        }
    }
    //Stuns the player
    public void StunPlayerWithIFrames(float LengthOfTime, Vector3 direction)
    {
        if (!Invincible)
        {
            arcadeVehicleController.StunnedActionFunction(true, direction, LengthOfTime);
            initalStun = Time.time;
            stunnedLength = LengthOfTime;
            SetUpInvincibiltyFrames(LengthOfTime + .5f);
        }
        //playerSpriteRenderer.sprite = SpriteImages[(int)SpriteStatus.DeliverStolen];
    }
    //Gives player invincibility
    public void SetUpInvincibiltyFrames(float LengthOfTime)
    {
        bubble.SetActive(true);
        Invincible = true;
        invincibiltyLength = LengthOfTime;
        initalInvincibilty = Time.time;
    }

    public void OrderPickUp()
    {
        //playerSpriteRenderer.sprite = SpriteImages[(int)SpriteStatus.DeliverAquired];
        initalOrderTime = Time.time;
        order = true;
    }

}
