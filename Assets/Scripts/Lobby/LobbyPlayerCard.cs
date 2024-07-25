using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spring.Runtime;
using UnityEngine.InputSystem;

//Left, Confirm, Right, Cancel

public class LobbyPlayerCard : MonoBehaviour
{
    [SerializeField] SpringToScale rightButtonSpring;
    [SerializeField] SpringToScale leftButtonSpring;

    public Animator animator;

    [SerializeField] LobbyCarMatSO lobbyCarSO;

    LobbyItemChanger<GameObject> carPrefabChanger;
    LobbyItemChanger<MatSelect> carMatUpdater;
    LobbyItemChanger<GameObject> characterPrefabChanger;
    LobbyItemChanger<GameObject> hatPrefabChanger;

    [SerializeField] Transform car;
    GameObject carBody;
    GameObject driver;
    GameObject hat;
    

    public int arrowLevel;
    public bool isReady;
    bool quitCheck;
    [SerializeField] TextMeshProUGUI readyText;
    [HideInInspector] public LobbyManager lobbyManager;
    [SerializeField] public List<RectTransform> cardPanels;
    [SerializeField] private Transform stagesHolder;
    [SerializeField] private RectTransform[] stagesVisual;
   
    [SerializeField] private Color stageColour = new Color(1,0,0,1);

    public List<Button> buttons;
    [SerializeField] List<CanvasGroup> buttonGroups;

    [SerializeField] GameObject CustomizeTextHolder;
    [SerializeField] GameObject ReadyTextHolder;

    public GameObject SwitchModel(GameObject currentModel, Transform parent)
    {

        if(parent.childCount > 0) DestroyImmediate(parent.GetChild(0).gameObject);
        return Instantiate(currentModel, parent);
    }
        

    private void Awake()
    {
        
        buttons = new List<Button>();
        buttons = GetComponentsInChildren<Button>().ToList();

        //stagesVisual = new RectTransform[stagesHolder.childCount];
        //for (int i = 0; i < stagesHolder.transform.childCount; i++)
        //{
        //    stagesVisual[i] = stagesHolder.transform.GetChild(i).GetComponent<RectTransform>();
        //}

        //stagesVisual[0].GetComponent<SpriteRenderer>().color = stageColour;
        //stagesVisual[0].localScale += new Vector3(10, 10, 0);

        //Set up car prefab selector
        carPrefabChanger = new LobbyItemChanger<GameObject>(lobbyCarSO.carPrefabs);
        carBody = Instantiate(carPrefabChanger.GetCurrentItem(), car);
        driver = carBody.GetComponent<CarVisualsHandler>().Driver;

        //Set up for the car material selector
        carMatUpdater = new LobbyItemChanger<MatSelect>(lobbyCarSO.carMatsLobby);       
        while (carMatUpdater.GetCurrentItem().active == true) carMatUpdater.IncreaseMeshIndex();
        carMatUpdater.GetCurrentItem().active = true;
        carBody.GetComponent<CarVisualsHandler>().carBodyMeshRendere.material = carMatUpdater.GetCurrentItem().mat;

        //Set up character selector
        characterPrefabChanger = new LobbyItemChanger<GameObject>(lobbyCarSO.characterPrefabs);

        //Set up Hat selector
        hatPrefabChanger = new LobbyItemChanger<GameObject>(lobbyCarSO.hatPrefabs);

        isReady = false;
        quitCheck = false;

        //sets initial stage for customization
        arrowLevel = 1;
        ReadyTextCheck();
        HideAllButtons();
        ShowArrowGroupButtons();
    }
    public void ResizeCardComps(Vector2 gridCellSize, int count)
    {

        //Multi-Select Box Sizing
        //X
        float horizontalSizeA = (2 * gridCellSize.x/3) - 80;
        
        cardPanels[0].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalSizeA);
        //Y
        float verticalSizeA = (3 * gridCellSize.y / (4));
        if (count == 2)
        {
            verticalSizeA /= 1.5f;
        }
        cardPanels[0].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalSizeA);

        //CarImagePanel Sizing
        //X
        float horizontalSizeB = (gridCellSize.x / 3);
        cardPanels[1].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalSizeB);
        //Y
        float verticalSizeB = (3 * gridCellSize.y / (4));
        verticalSizeB *= 1.1f;
        verticalSizeB = Mathf.Clamp(verticalSizeB, 0, 200);

        cardPanels[1].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalSizeB);        
    }

    public void LeftButton(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        leftButtonSpring.Nudge(10 * Vector3.one);
        
        if (arrowLevel == 1)
        {

            //Change current car model
            carPrefabChanger.DecreaseMeshIndex();
            //Replaces current model with the new car model on screen
            carBody = SwitchModel(carPrefabChanger.GetCurrentItem(), car);
            //Sets the material for the new model to be the current car material
            carBody.GetComponent<CarVisualsHandler>().carBodyMeshRendere.material = carMatUpdater.GetCurrentItem().mat;
            //Gets the gameobject which will be used to instantiate the character prefab
            driver = carBody.GetComponent<CarVisualsHandler>().Driver;
          
        }
        else if (arrowLevel == 2)
        {
        
            //Sets the current material active to be false to allow other players to select it
            carMatUpdater.GetCurrentItem().active = false;
            //Checks for a available Car Material
            do
                carMatUpdater.DecreaseMeshIndex();
            while (carMatUpdater.GetCurrentItem().active == true);
            //Applies the new material to the car in scene
            carBody.GetComponent<CarVisualsHandler>().carBodyMeshRendere.material = carMatUpdater.GetCurrentItem().mat;
            //Sets the active state of the material to true to avoid other players selecting it
            carMatUpdater.GetCurrentItem().active = true;
        }
        else if (arrowLevel == 3)
        {
        
            //Changes the current character prefab
            characterPrefabChanger.DecreaseMeshIndex();
            //Replaces the character on screen with the new character and stores the Hat Gameobject holder
            hat = SwitchModel(characterPrefabChanger.GetCurrentItem(), driver.transform).transform.GetChild(0).gameObject;
            //Spawns the hat on the new character prefab
            SwitchModel(hatPrefabChanger.GetCurrentItem(), hat.transform);
        }
        else if (arrowLevel == 4)
        {
            //Changes the current hat model
            hatPrefabChanger.DecreaseMeshIndex();
            //Replaces the current hat model with new hat model in scene
            SwitchModel(hatPrefabChanger.GetCurrentItem(), hat.transform);
        }
    }
    public void RightButton(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        rightButtonSpring.Nudge(10 * Vector3.one);

        if (arrowLevel == 1)
        {
         

            //Change current car model
            carPrefabChanger.IncreaseMeshIndex();
            //Replaces current model with the new car model on screen
            carBody = SwitchModel(carPrefabChanger.GetCurrentItem(), car);
            //Sets the material for the new model to be the current car material
            carBody.GetComponent<CarVisualsHandler>().carBodyMeshRendere.material = carMatUpdater.GetCurrentItem().mat;
            //Gets the gameobject which will be used to instantiate the character prefab
            driver = carBody.GetComponent<CarVisualsHandler>().Driver;

        }
        else if (arrowLevel == 2)
        {
            
            //Sets the current material active to be false to allow other players to select it
            carMatUpdater.GetCurrentItem().active = false;
            //Checks for a available Car Material
            do
                carMatUpdater.IncreaseMeshIndex();
            while (carMatUpdater.GetCurrentItem().active == true);
            //Applies the new material to the car in scene
            carBody.GetComponent<CarVisualsHandler>().carBodyMeshRendere.material = carMatUpdater.GetCurrentItem().mat;
            //Sets the active state of the material to true to avoid other players selecting it
            carMatUpdater.GetCurrentItem().active = true;
        }
        else if (arrowLevel == 3)
        {
          
            //Changes the current character prefab
            characterPrefabChanger.IncreaseMeshIndex();
            //Replaces the character on screen with the new character and stores the Hat Gameobject holder
            hat = SwitchModel(characterPrefabChanger.GetCurrentItem(), driver.transform).transform.GetChild(0).gameObject;
            //Spawns the hat on the new character prefab
            SwitchModel(hatPrefabChanger.GetCurrentItem(), hat.transform);
        }
        else if (arrowLevel == 4)
        {
            //Changes the current hat model
            hatPrefabChanger.IncreaseMeshIndex();
            //Replaces the current hat model with new hat model in scene
            SwitchModel(hatPrefabChanger.GetCurrentItem(), hat.transform);
        }
    }
    public void ConfirmButton(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].GetComponent<SpriteRenderer>().color = Color.white;
        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].localScale -= new Vector3(10, 10, 0);
        arrowLevel++;
        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].GetComponent<SpriteRenderer>().color = stageColour;
        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].localScale += new Vector3(10, 10, 0);
        ArrowLevelCheck();
        lobbyManager.CheckAllReady();

    }
    public void CancelButton(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        //Debug.Log("Cancel Button Press");
        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].GetComponent<SpriteRenderer>().color = Color.white;
        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].localScale -= new Vector3(10, 10, 0);
        arrowLevel--;
        //stagesVisual[Mathf.Clamp(arrowLevel-1, 0, 3)].GetComponent<SpriteRenderer>().color = stageColour;
        //stagesVisual[Mathf.Clamp(arrowLevel - 1, 0, 3)].localScale += new Vector3(10, 10, 0);
        ArrowLevelCheck();
    }

    //Displays the text for the current stage of customization
    private void ReadyTextCheck()
    {
        animator.SetInteger("ArrowLevel", arrowLevel);

        //if (arrowLevel == 1)
        //{
        //    readyText.text = "Car";
        //}
        //else if (arrowLevel == 2)
        //{
        //    readyText.text = "Colour";
        //}
        //else if (arrowLevel == 3)
        //{
        //    readyText.text = "Character";
        //}
        //else if (arrowLevel == 4)
        //{
        //    readyText.text = "Hat";

        //}
        //else if (arrowLevel == 5)
        //{
        //    readyText.text = "Ready";

        //}

    }
    private void ArrowLevelCheck()
    {
        isReady = false;
        arrowLevel = Mathf.Clamp(arrowLevel, 0, 6);

        //Functionality for switching between customization selectors
        switch (arrowLevel) {
            //Car Stage
            case 1:
                break;
            case 2:              
                 if (driver.transform.childCount == 0) break;
                Destroy(driver.transform.GetChild(0).gameObject);
                
                break;
            case 3:
                if (driver.transform.childCount > 0) break;
                hat = Instantiate(characterPrefabChanger.GetCurrentItem(), driver.transform).transform.GetChild(0).gameObject;
                Instantiate(hatPrefabChanger.GetCurrentItem(), hat.transform);
                break;
            case 4:

                break;
        }

        HideAllButtons();
        ReadyTextCheck();
        if (arrowLevel < 5)
        {
            //readyText.text = "Not Ready";
            ShowArrowGroupButtons();
            ReadyTextHolder.SetActive(false);
            CustomizeTextHolder.SetActive(true);
            //buttons[2].gameObject.SetActive(true);
        }
        else
        {
            isReady = true;
            CustomizeTextHolder.SetActive(false);
            ReadyTextHolder.SetActive(true);
            //buttons[2].gameObject.SetActive(false);
        }
        if (arrowLevel == 0)
        {
            if (lobbyManager.gridManager.playerCards.Count > 1)
            {
                lobbyManager.gridManager.RemovePlayerCard(this);
                Destroy(this.gameObject);
            }
            else
            {
                GameManager.instance.ChangeScene(3);
            }
        }        
        
    }
    private void HideAllButtons()
    {
        foreach (CanvasGroup group in buttonGroups)
        {
            group.alpha = 0;
        }        
    }
    
    private void ShowArrowGroupButtons()
    {
        //buttonGroups[arrowLevel].alpha = 1.0f;
        buttonGroups[0].alpha = 1.0f;
    }

    //GETTERS

    public Material GetCarMaterial()
    {
        return lobbyCarSO.carMatsInGame[carMatUpdater.GetCurrentIndex()];
    }
    //Returns the pregab to be used for the car
    public GameObject GetCarPrefab()
    {
        return carPrefabChanger.GetCurrentItem();
    }

    public Material GetIconMaterial()
    {
        return lobbyCarSO.characterIconMat[characterPrefabChanger.GetCurrentIndex()];
    }
    //Returns the prefab for the character
    public GameObject GetCharacterPrefab()
    {
        return characterPrefabChanger.GetCurrentItem();
    }

    public GameObject GetCharacterHat()
    {
        return hatPrefabChanger.GetCurrentItem();
    }

    private void OnDestroy()
    {
        carMatUpdater.GetCurrentItem().active = false;
    }

    //Used to spawn the full player
    void SpawnPlayerFullMesh()
    {
        carBody = SwitchModel(carPrefabChanger.GetCurrentItem(), car);
        carBody.GetComponent<CarVisualsHandler>().carBodyMeshRendere.material = carMatUpdater.GetCurrentItem().mat;
        hat = SwitchModel(characterPrefabChanger.GetCurrentItem(), driver.transform).transform.GetChild(0).gameObject;
        SwitchModel(hatPrefabChanger.GetCurrentItem(), hat.transform);
    }
}
