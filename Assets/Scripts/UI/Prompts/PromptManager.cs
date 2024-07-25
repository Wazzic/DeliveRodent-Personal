using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PromptManager : MonoBehaviour
{
    public bool firstDelivery;

    AnalogueClock analogueClock;

    public List<PromptHandler> promptHandlers { get; private set; }

    public static PromptManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        firstDelivery = false;  
        
        promptHandlers = new List<PromptHandler>();
    }
    private void Start()
    {
        StartCoroutine(FindPromptHandlers());
    }


    private IEnumerator FindPromptHandlers()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        promptHandlers = FindObjectsOfType<PromptHandler>().ToList();
        if (promptHandlers != null)
        {
            Debug.Log("Prompt handlers located");
        }
    }
    public void ShowPromptToAll(int index)
    {
        foreach (PromptHandler handler in promptHandlers)
        {
            handler.ShowPromptindex(index);
        }
    }
}
