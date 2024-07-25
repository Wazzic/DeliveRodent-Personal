using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core.Environments;

public class AnalyticsManager : MonoBehaviour
{
    [HideInInspector] public int playerNumber;
    [HideInInspector] public int roundTime;
    [HideInInspector] public int activeZones;
    [HideInInspector] public int steals;
    [HideInInspector] public int successfulDeliveries;
    [HideInInspector] public int failedDeliveries;

    [HideInInspector] public int ballsFired;
    [HideInInspector] public int ballsHit;
    [HideInInspector] public int minesFired;
    [HideInInspector] public int minesHit;
    [HideInInspector] public int boostItemsUsed;

    public ObstacleHeatMap obstacleHeatMap;
    //[SerializeField] private string environmentName = "RocketRaccoonFactory";
    public static AnalyticsManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        //Initialise();
        //Analytics.initializeOnStartup = false;
        ResetValues();

        obstacleHeatMap = GetComponent<ObstacleHeatMap>();
    }
    public async void Start()
    {
        try
        {
            //InitializationOptions options = new InitializationOptions();

            //options.SetEnvironmentName(environmentName);

            await UnityServices.InitializeAsync();
            Debug.Log(UnityServices.State);
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            //TODO: actually deal with this...
        }

        //Debug.Log("Unity Analytics initialised");
    }
    public void EnableAnalytics()
    {
        
        
        //AnalyticsService.Instance.ProvideOptInConsent()
        //Analytics.ResumeInitialization();
        //AnalyticsService.Instance.SetAnalyticsEnabled(true);
        //AnalyticsService.Instance.StartDataCollection();        
    }
    private void ResetValues()
    {
        playerNumber = -1;
        roundTime = -1;
        activeZones = -1;
        steals = 0;
        successfulDeliveries = 0;
        failedDeliveries = 0;

        ballsFired = 0;
        ballsHit = 0;
        minesFired = 0;
        minesHit = 0;
        boostItemsUsed = 0;
    }
    public void SendEndGameAnalytics()
    {
        //Debug.Log("Sending end game Events");
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "numberOfPlayers", playerNumber },
            { "roundTime", roundTime },
            { "activeZones", activeZones },
            { "numberOfSteals", steals },
            { "numberOfDeliveries", successfulDeliveries },
            { "failedDeliveries", failedDeliveries }
        };
        AnalyticsService.Instance.CustomData("GameOver", parameters);
    }
    public void SendEndGameItemAnalytics()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "numberOfBallsFired", ballsFired },
            { "numberOfBallsHit", ballsHit },
            { "numberOfMinesFired", minesFired },
            { "numberOfMinesHit", minesHit },
            { "numberOfBoostItems", boostItemsUsed }
        };
        AnalyticsService.Instance.CustomData("EndGameItems", parameters);
    }
    public void DeliveryCompletedEvent(float remainingTime, float totalDeliveryTime)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "numberOfPlayers", playerNumber },
            { " roundTime", roundTime },
            { "activeZones", activeZones },
            { "completedDeliveryTime", totalDeliveryTime - remainingTime },
            { "remainingDeliveryTime", remainingTime }
        };
        AnalyticsService.Instance.CustomData("SuccessfulDelivery", parameters);
    }
}
