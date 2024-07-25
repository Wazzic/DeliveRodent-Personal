using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypingSceneManager : MonoBehaviour
{
    [SerializeField] PlayerConfigs playerConfigs;
    [SerializeField] ZoneMatSO zoneMats;

    [SerializeField] List<GameObject> kartPrefabs;
    int currentNumberOfPlayers = 0;
    [SerializeField] GameObject ScoreTextPrefab;

    ScoreManager scoreManager;
    DeliveryManager deliveryManager;
    InventoryUI inventoryUI;

    [SerializeField] List<Transform> spawnPoints;
    public static PrototypingSceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        //PlayerAmount = 1;

        scoreManager = this.GetComponent<ScoreManager>();
        //scoreManager.SetTextPrefab(ScoreTextPrefab);
        deliveryManager = this.AddComponent<DeliveryManager>();
        deliveryManager.ActiveAmount = 1;
        deliveryManager.zoneMaterials = zoneMats;
        inventoryUI = this.AddComponent<InventoryUI>();
    }

    public void SpawnPlayer(PlayerInput input)
    {
        GameObject playerCar = PlayerInput.Instantiate(kartPrefabs[currentNumberOfPlayers - 1], controlScheme: "GamePad", pairWithDevice: input.GetComponent<InputDevice>()).gameObject;
        playerCar.transform.position = spawnPoints[currentNumberOfPlayers - 1].position;
        playerCar.name = "Player" + (currentNumberOfPlayers - 1).ToString();
        playerCar.GetComponent<PlayerLocalManager>().ID = currentNumberOfPlayers - 1;
        scoreManager.AddPlayerScoreScript(playerCar.GetComponent<PlayerScore>());
        currentNumberOfPlayers++;
    }
}
