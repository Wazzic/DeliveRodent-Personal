using Spring.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;

public class PlayerItemsHandler : MonoBehaviour
{
    private bool itemButtonPressed;
    private bool hasItem;
    private int currentItemIndex;

    [Header("Cooldown Settings")]
    private float itemCooldown = 25f;
    float itemTimer = 0f;
    [SerializeField] float obstacleCooldownReduction = 1f;  //the amount that breaking an obstacle reduces cd
    [SerializeField] TextMeshProUGUI itemCoolDownText;
    [SerializeField] Slider cooldownSlider;
    SpringToScale itemIconSpring;
    [SerializeField] CanvasGroup cooldownReductionFlash;
    [Header("Sounds")]
    [SerializeField] AudioSource oneTimeAudioSource;
    [SerializeField] AudioClip bombInitSound;
    [SerializeField] AudioClip mineInitSound;
    [SerializeField] AudioClip speedBoostInitSound;
    [SerializeField] AudioClip shieldInitSound;
    [SerializeField] AudioClip missileInitSound;

    SpringToScale itemSpringToScale;
    [Header("Images")]
    [SerializeField] Image itemIconHolder;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite noItemSprite;
    [Header("BallOfDeath")]
    [SerializeField] Transform ballOfDeathFirePoint;
    [SerializeField] float ballOfDeathSpeed;
    [SerializeField] Rigidbody ballOfDeath;
    [Header("Mines")]
    [SerializeField] Transform minesFirePoint;
    [SerializeField] float mineSpeed;
    [SerializeField] Rigidbody mines;
    [Header("Missile")]
    [SerializeField] Transform missileFirePoint;
    [SerializeField] GameObject missilePrefab;

    public  PlayerScore[] playerScoreScripts;
    private PlayerScore myPlayerScoreScript;

    DeliveryVFXHandler vFXHandler;
    private void Start()
    {
        //playerScore = GetComponent<PlayerScore>();

        itemIconHolder.sprite = noItemSprite;
        itemSpringToScale = itemIconHolder.GetComponent<SpringToScale>();
        itemIconSpring = cooldownSlider.GetComponent<SpringToScale>();

        vFXHandler = GetComponentInChildren<DeliveryVFXHandler>();

        InvokeRepeating("SpringScaleImage", 1f, 1f);

        myPlayerScoreScript = GetComponent<PlayerScore>();
        GameObject[] tempPlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        playerScoreScripts = new PlayerScore[tempPlayerObjects.Length];

        for (int i = 0; i < playerScoreScripts.Length; i++)
        {
            playerScoreScripts[i] = tempPlayerObjects[i].GetComponent<PlayerScore>();
        }

        obstacleCooldownReduction = 1f;        
    }
    private void ReceiveItem(int index)
    {
        currentItemIndex = index;
        // Update UI item icon
        itemIconHolder.sprite = sprites[index];
        SpringScaleImage();
        hasItem = true;
    }
    void SpringScaleImage()
    {
        if (hasItem)
        {
            itemSpringToScale.Nudge(new Vector3(4, 4, 4));
        }
    }
    private void UseItem()
    {
        itemIconHolder.sprite = noItemSprite;
        hasItem = false;
        itemTimer = 0;
    }

    public void OnitemButton(InputAction.CallbackContext context)
    {
        itemButtonPressed = context.action.WasPressedThisFrame();
    }
    
    void Update()
    {
        if (hasItem)
        {
            itemCoolDownText.enabled = false;
            if (itemButtonPressed)
            {
                UseItem();

                switch (currentItemIndex)
                {
                    case 0: Mines(); PlaySound(mineInitSound); break;
                    case 1: BallOfDeath(); PlaySound(bombInitSound); break;
                    case 2: SpeedBoost(); PlaySound(speedBoostInitSound); break;
                    case 3: Shield(); PlaySound(shieldInitSound); break;
                    case 4: Missile(); PlaySound(missileInitSound); break;
                    //case 3: Jump(); break;
                }
            }
        }
        else
        {
            itemCoolDownText.enabled = true;
            if (itemTimer <= itemCooldown)
            {
                float countdownFloat = Mathf.Lerp(itemCooldown, 0f, itemTimer / itemCooldown);
                int countdownInt = Mathf.RoundToInt(countdownFloat);
                itemCoolDownText.text = countdownInt.ToString();
                itemTimer += Time.deltaTime;
                cooldownSlider.value = Mathf.Lerp(1f, 0f, itemTimer / itemCooldown);
            }
            else
            {
                SetNewCoolDown();
                int itemRange = playerRank > 0 ? 5 : 4;
                ReceiveItem(Random.Range(0, itemRange));                                
            }
        }
        itemButtonPressed = false;

    }

    public int playerRank = -1;
    public void PlayerRank()
    {
        BubbleSortPlayers();
        for (int i = 0; i < playerScoreScripts.Length; i++)
        {
            if (playerScoreScripts[i].GetScore() == myPlayerScoreScript.GetScore())
            {
                playerRank = i;
                break;
            }
        }        
    }
    
    public void SetNewCoolDown()
    {
        // Bubble Sort players by score (descending order)
        //BubbleSortPlayers();
        PlayerRank();

        // Find the rank of the current player score
        /*
        playerRank = -1;
        for (int i = 0; i < playerScoreScripts.Length; i++)
        {
            if (playerScoreScripts[i].GetScore() == myPlayerScoreScript.GetScore())
            {
                playerRank = i;
                break;
            }
        }
        */
        if (playerRank != -1)
        {
            // Assign cooldown values based on ranks, considering ties
            switch (playerRank)
            {
                case 0: // 1st place
                    itemCooldown = 15.0f;
                    break;
                case 1: // 2nd place
                    itemCooldown = 10.0f;
                    break;
                case 2: // 3rd place
                    itemCooldown = 7.5f;
                    break;
                case 3: // 4th place
                    itemCooldown = 5f;
                    break;
                default:
                    Debug.LogError("Something wrong has happened");
                    break;
            }
        }
        else
        {
            Debug.LogError("Player score not found in playerScores array");
        }
    }

    private void BubbleSortPlayers()
    {
        //sort the playerscore array so the winning player has the 
        int n = playerScoreScripts.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (playerScoreScripts[j].GetScore() < playerScoreScripts[j + 1].GetScore())
                {
                    // Swap players
                    PlayerScore temp = playerScoreScripts[j];
                    playerScoreScripts[j] = playerScoreScripts[j + 1];
                    playerScoreScripts[j + 1] = temp;
                }
            }
        }
    }
    private void BallOfDeath()
    {
        Rigidbody ball = Instantiate(ballOfDeath, ballOfDeathFirePoint.position, GetRandomRotation());
        ball.velocity = transform.forward * ballOfDeathSpeed;
        ball.GetComponent<BallOfDeathScript>().PlayerCollider = GetComponent<Collider>();
    }
    private void Shield()
    {
        GetComponent<PlayerStatus>().SetUpInvincibiltyFrames(2);
        vFXHandler.PlayShieldEffect();
    }

    private void Mines()
    {
        Rigidbody mine = Instantiate(mines, minesFirePoint.position, GetRandomRotation());
        //mine.velocity = transform.forward * -mineSpeed;
        mine.GetComponent<MineScript>().PointSpawnedFrom = minesFirePoint;
    }
    private void SpeedBoost()
    {
        GetComponent<ArcadeVehicleController>().BoostPowerUpFunction();
    }
    private void Jump()
    {
        GetComponent<ArcadeVehicleController>().JumpFunction();
    }
    private void Missile()
    {
        GameObject newMissile = Instantiate(missilePrefab, missileFirePoint.position, missileFirePoint.rotation);
        newMissile.GetComponent<HomingMissile>().firingPlayer = transform.root.GetComponent<Rigidbody>();
    }
    private Quaternion GetRandomRotation()
    {
        float randRot = Random.Range(0f, 360f);

        return Quaternion.Euler(randRot, randRot, randRot);
    }
    public void ReduceItemCD()
    {
        itemTimer += obstacleCooldownReduction;
        itemIconSpring.Nudge(Vector3.one * 15f);
        cooldownReductionFlash.DOFade(1, 0.5f).SetLoops(0, LoopType.Yoyo).SetEase(Ease.InOutCubic);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        oneTimeAudioSource.PlayOneShot(clip);
    }

}
