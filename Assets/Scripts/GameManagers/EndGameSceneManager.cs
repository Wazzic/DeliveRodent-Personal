using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSceneManager : MonoBehaviour
{
    //Sends back to main menu by calling Game Manager
    public void MainMenu()
    {
        GameManager.instance.ChangeScene(7);
    }
}
