using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PromptController : MonoBehaviour
{
    public List<Prompt> playerPrompts = new List<Prompt>();
    void Awake()
    {
         playerPrompts = GetComponentsInChildren<Prompt>().ToList();
    }
    private void Start()
    {
        HideAllPrompts();
    }


    public void ShowTextPrompts()
    {
        //playerPrompts[0].ShowPrompt();
    }
    public void ShowImagePrompts()
    {
        //playerPrompts[1].ShowPrompt();
    }
    public void ShowTextandImageprompts()
    {
        //playerPrompts[2].ShowPrompt();
    }
    public void HideAllPrompts()
    {
        foreach(Prompt prompt in playerPrompts)
        {
            prompt.HidePrompt();
        }
    }
}
