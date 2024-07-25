using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
//using Lofelt.NiceVibrations;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    public PlayerInputManager playerInputManager;
    private ArcadeVehicleController vehicleController;
    [SerializeField] public List<PlayerInput> playerInput;
    private InputAction01 playerInputActions;
    [SerializeField] private List<CullingGroup> playerCullingGroups;
    public bool controlsEnabled;

    // Start is called before the first frame update
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
        playerInput = new List<PlayerInput>();
        DisablePlayerControls();
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    public void OnNewInput(PlayerInput input)
    {
        Debug.Log("NewInputTriggered");
        playerInput.Add(input);
        if (PrototypingSceneManager.instance != null)
        {
            PrototypingSceneManager.instance.SpawnPlayer(input);
        }
        

        /*
        CinemachineVirtualCamera virtualCamera = input.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.gameObject.layer = 9 + playerInput.Count;
        */
    }

    public void OnPlayerLeave(PlayerInput input)
    {
        Debug.Log("NewInputTriggered");
        playerInput.Remove(input);

        /*
        CinemachineVirtualCamera virtualCamera = input.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.gameObject.layer = 9 + playerInput.Count;
        */
    }

    public void EnablePlayerControls()
    {
        controlsEnabled = true;        
    }    
    public void DisablePlayerControls()
    {
        controlsEnabled = false;
    }
    
}
