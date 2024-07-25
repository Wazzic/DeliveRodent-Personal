using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimePromptController : MonoBehaviour
{
    public Prompt prompt;
    private void Awake()
    {
        prompt = GetComponent<Prompt>();
    }
    public void ShowOvertime()
    {
        prompt.interruptToken = true;
        prompt.ShowPromptSpringTo();
    }
}
