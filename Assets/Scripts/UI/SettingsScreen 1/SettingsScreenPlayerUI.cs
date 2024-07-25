using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsScreenPlayerUI : MonoBehaviour
{
    private void Start()
    {
        InputManager.instance.OnNewInput(this.GetComponent<PlayerInput>());
    }
}
