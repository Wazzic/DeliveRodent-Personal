using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PromptHandler : MonoBehaviour
{
    [SerializeField] Prompt[] prompts;
    /*
    0 = starting pick-up
    1 = Arrow to pick-up
    2 = Arrow to drop off
    3 =  1st Delivery Dropped off (All)
    4 = 1st Steal
    5 = 
    6 = 
    7
    TODO - arrow pointing to drop off, 1st delivery (player who did it), 1st delivery (players who didnt do it)
    chase delivery, Steal, Stolen, Slipstream, Drifting, Overtime
    */

    public bool hasPickedUp;
    public bool hasDropedOff;
    public bool hasChasedDelivery;
    public bool hasStolen;
    public bool hasSlipstream;
    public bool hasDrifted;
    
    private void Start()
    {
        StartCoroutine(Delay(1f));
        //prompts[0].ShowPrompt();
    }
    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ShowPromptindex(0);

    }
    public void ShowPromptindex(int index)
    {
        prompts[index].ShowPrompt();
    }
    public void HidePromptindex(int index)
    {
        prompts[index].HidePrompt();
    }
}
