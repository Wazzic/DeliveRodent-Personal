using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TestScene01Manager : MonoBehaviour
{
    [SerializeField] PlayerConfigs playerConfigsSO;
    [SerializeField] ZoneMatSO zoneMatSO;
    public int PlayerAmount;
    //[SerializeField] int SetActiveNum; //Not used I think
    [SerializeField] List<GameObject> kartPrefabs;
    [SerializeField] GameObject ScoreTextPrefab;

    public List<Transform> PlayerCars;  //{ get; private set; }

    ScoreManager scoreManager;
    DeliveryManager deliveryManager;
    InventoryUI inventoryUI;

    [SerializeField] List<Transform> spawnPoints;
    public static TestScene01Manager instance = null;
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
        PlayerAmount = playerConfigsSO.numberOfPlayers;
        if (PlayerAmount < 1)
        {
            PlayerAmount = 1;
        }

        scoreManager = this.AddComponent<ScoreManager>();
        scoreManager.SetTextPrefab(ScoreTextPrefab);
        deliveryManager = this.AddComponent<DeliveryManager>();
        deliveryManager.ActiveAmount = playerConfigsSO.activeZones;
        deliveryManager.zoneMaterials = zoneMatSO;
        inventoryUI = this.AddComponent<InventoryUI>();

        PlayerCars = new List<Transform>();
    }
    
    void Start()
    {        
        //ChangeCharacter();
        //spawnPointsList = GetComponentsInChildren<Transform>().ToList();
        //spawnPointsList.RemoveAt(0);
        //spawnPointsList.RemoveAt(0);
        SpawnPlayers();
       

        //AnalyticsManager.instance.playerNumber = PlayerAmount;
        //AnalyticsManager.instance.roundTime = playerConfigsSO.roundTime;
        //AnalyticsManager.instance.activeZones = playerConfigsSO.activeZones;
    }
    public void SpawnPlayers()
    {
        for (int i = 0; i < PlayerAmount; i++)
        {
            //GameObject playerCar = Instantiate(kartPrefabs[i], spawnPoints[i].position, Quaternion.identity);
            //InputManager.instance.OnNewInput(playerCar.GetComponent<PlayerInput>());

            //Spawn and sets the controller to be used for the player
            GameObject playerCar = PlayerInput.Instantiate(kartPrefabs[i], controlScheme: "GamePad" , pairWithDevice: playerConfigsSO.controllers[i]).gameObject;
            //Keeps a reference to all player transforms
            PlayerCars.Add(playerCar.transform);
            //Spawns the car and attaches it to the player for visual representation
            GameObject playerCarVisual = Instantiate(playerConfigsSO.carPrefabs[i], playerCar.transform);
            //Spawns the character and attaches it to the driver seat
            Instantiate(playerConfigsSO.characterPrefabs[i], playerCarVisual.GetComponent<CarVisualsHandler>().Driver.transform);
            GameObject hat = Instantiate(playerConfigsSO.characterHats[i], playerCarVisual.GetComponent<CarVisualsHandler>().Driver.transform.GetChild(0).GetChild(0));
            hat.GetComponent<MeshRenderer>().material = zoneMatSO.defaultHatMat;
            UpdatePlayerVisuals(playerCar, playerCarVisual, i);

            //playerCar.GetComponent<PlayerScore>().scoreText.color = playerConfigsSO.carMaterials[i].GetColor("_ColorDim");
            playerCar.transform.position = spawnPoints[i].position;
            playerCar.transform.rotation = spawnPoints[i].rotation;
            if(spawnPoints != null)
            {
                //playerCar.transform.position = spawnPoints[i].position;
            }
            playerCar.name = "Player" + i;
            playerCar.GetComponent<PlayerLocalManager>().ID = i;
            scoreManager.AddPlayerScoreScript(playerCar.GetComponent<PlayerScore>());

            playerCar.GetComponentInChildren<PlayerUIHandler>().foodSliders = playerCarVisual.GetComponentsInChildren<Slider>().ToList();   //Setting the food sliders

            Canvas[] delCanvas = playerCarVisual.GetComponentsInChildren<Canvas>();
            foreach (Canvas canvas in delCanvas)
            {
                Camera camera = playerCar.GetComponentInChildren<Camera>();
                canvas.worldCamera = camera;
                canvas.GetComponent<DeliveryTimerWorldSpaceCanvas>().playerCam = camera;
            }          

            //playerCar.GetComponent<RumbleController>().SetPlayerID(i);
            scoreManager.playerItemsHandlers.Add(playerCar.GetComponent<PlayerItemsHandler>());
        }
    }
    //Updates the car 
    private void UpdatePlayerVisuals(GameObject car, GameObject player, int currentPlayer)
    {
        player.transform.Find("CarBody/Body").GetComponent<MeshRenderer>().material = playerConfigsSO.carMaterials[currentPlayer];
        car.transform.GetComponent<Delivery>().PlayerIcon = playerConfigsSO.characterIcon[currentPlayer];
    }
}