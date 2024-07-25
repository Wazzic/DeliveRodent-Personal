using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class RumbleController : MonoBehaviour
{
    int playerID;
    bool hapticsOnStart;
    float hapticLevel = 0.5f;
    private void Awake()
    {
        hapticLevel = GameManager.instance.playerConfigs.hapticLevel;
    }

    private void Start()
    {        
        EnableHaptics(true);
        SetHapticLevel();
    }
    public void EnableHaptics(bool check)
    {
        GamepadRumbler.SetCurrentGamepad(playerID);
        HapticController.hapticsEnabled = check;
    }
    public void SetHapticLevel()
    {
        GamepadRumbler.SetCurrentGamepad(playerID);
        HapticController.outputLevel = GameManager.instance.playerConfigs.hapticLevel * 0.5f;
    }
    public void SetPlayerID(int id)
    {
        playerID = id;
    }
    public void PlayRumble(float amplitude, float frequency, float duration)
    {
        GamepadRumbler.SetCurrentGamepad(playerID);
        HapticPatterns.PlayConstant(amplitude * GameManager.instance.playerConfigs.hapticLevel, frequency, duration);
    }
    public void PlayOneShot(float amplitude, float frequency)
    {
        GamepadRumbler.SetCurrentGamepad(playerID);
        HapticPatterns.PlayEmphasis(amplitude * GameManager.instance.playerConfigs.hapticLevel, frequency);
    }
    
}
