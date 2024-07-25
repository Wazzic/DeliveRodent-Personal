using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update

    /*TODO: 
     *  The screen should fade to black on the previous screen - this prepares it for this scene.
     * 
        The previous scene number needs passed into this script so that the scenes can correctly called.
        Once the scene number has been passed in - call a function to calculate the next scene destination.
        A loading icon should then be displayed and animated for like 3 seconds
        Once the time is up - the scene should then change and fade into the correct scene.


        Lobby -> Game = Needs to go to scene 5
        Game -> End = Needs to be scene 7
        End -> Loby needs to be scene 1
    */
    [SerializeField] float waitFor = 7f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.ChangeScene(5);
        }
        if (waitFor <= 0f)
        {
            GameManager.instance.ChangeScene(5);
            waitFor = 8.0f;
        }
        waitFor -= Time.deltaTime;
    }
}
