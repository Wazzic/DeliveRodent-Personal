using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FlatKit;

public class LobbyManager : MonoBehaviour
{
    //[SerializeField] public CanvasGroup quitText;
    [SerializeField] private LobbyCarMatSO carMatSO;
    [SerializeField] PlayerConfigs playerConfigs;
    [SerializeField] public LobbyGridManager gridManager;
    [SerializeField] GameObject playerUIPrefab;
    List<LobbyPlayerCard> lobbyCards;
    private void Start()
    {
        InputManager.instance.playerInputManager.splitScreen = false;
        InputManager.instance.playerInputManager.playerPrefab = playerUIPrefab;

       foreach (MatSelect currentMat in carMatSO.carMatsLobby)
        {
            currentMat.active = false;
        }

        lobbyCards = new List<LobbyPlayerCard>();
        playerConfigs.controllers = new List<InputDevice>();
        //GetComponent<PlayerInputManager>().JoinPlayer();
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoin");
        
        gridManager.HideJoinPanel();
        //playerConfigs.controllers.Add(playerInput.devices[0]);
        InputManager.instance.OnNewInput(playerInput);
        LobbyPlayerCard newCard = gridManager.AddPlayerCard(playerInput.gameObject.GetComponent<LobbyPlayerCard>());
        //LobbyPlayerCardController newController = playerInput.GetComponent<LobbyPlayerCardController>();
        
        //newController.card = newCard; 
        //newController.gridManager = gridManager;
        //newCard.playerCardController = newController;
        newCard.lobbyManager = this;
        
    }
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left");
        //gridManager.ResizeGrid();
        InputManager.instance.OnPlayerLeave(playerInput);
        gridManager.ShowJoinPanel();
    }
    public void CheckAllReady()
    {
        bool check = true;
        foreach (LobbyPlayerCard player in gridManager.playerCards)
        {
            if (!player.isReady)
            {
                check = false;
            }
        }
        if (check)
        {
            for(int i = 0; i < InputManager.instance.playerInput.Count; i++)
            {
                InputManager.instance.playerInput[i].currentActionMap.Disable();
            }
            InputManager.instance.playerInputManager.DisableJoining();

            //Used to store the order of controllers for correct splitscreen location
            foreach (LobbyPlayerCard player in gridManager.playerCards)
            {
                playerConfigs.controllers.Add(player.gameObject.GetComponent<PlayerInput>().devices[0]);
            }

            Debug.Log("All Ready");
            CarSettings();
            /*
            foreach (LobbyPlayerCard player in gridManager.playerCards)
            {
                Destroy(player.gameObject);
            }
            */
            GameManager.instance.ChangeScene(10);
        }
    }
    private void CarSettings()
    {
        playerConfigs.carModels = new List<Mesh>();
        playerConfigs.carMaterials = new List<Material>();
        playerConfigs.carPrefabs = new List<GameObject>();

        playerConfigs.characterModels = new List<Mesh>();
        playerConfigs.characterMaterials = new List<Material>();
        playerConfigs.characterPrefabs = new List<GameObject>();
        playerConfigs.characterIcon = new List<Material>();
        playerConfigs.characterHats = new List<GameObject>();

        playerConfigs.numberOfPlayers = gridManager.playerCards.Count;
        for (int i = 0; i < gridManager.playerCards.Count; i++)
        {
            //playerConfigs.carModels.Add(gridManager.playerCards[i].GetCarMesh());
            playerConfigs.carMaterials.Add(gridManager.playerCards[i].GetCarMaterial());
            playerConfigs.carPrefabs.Add(gridManager.playerCards[i].GetCarPrefab());

            //playerConfigs.characterModels.Add(gridManager.playerCards[i].GetCharacterMesh());
            //playerConfigs.characterMaterials.Add(gridManager.playerCards[i].GetCharacterMaterial());
            playerConfigs.characterPrefabs.Add(gridManager.playerCards[i].GetCharacterPrefab());
            playerConfigs.characterIcon.Add(gridManager.playerCards[i].GetIconMaterial());
            playerConfigs.characterHats.Add(gridManager.playerCards[i].GetCharacterHat());
        }
    }
}
