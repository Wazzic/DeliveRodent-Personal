using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spring;
using Spring.Runtime;

public class PlayerUIHandler : MonoBehaviour
{
    //public DeliveryTimerWorldSpaceCanvas[] delCanvas;
    
    public List<Slider> foodSliders;
    public List<CanvasGroup> foodTimerCanvasGroups;
    List<SpringToScale> scaleSprings;

    [SerializeField] RectTransform LockOnImage;

    private void Start()
    {
        /*
        delCanvas = FindObjectsOfType<DeliveryTimerWorldSpaceCanvas>();
        foreach (DeliveryTimerWorldSpaceCanvas canvas in delCanvas)
        {
            GameObject foodTimer = Instantiate(foodTimerPrefab, canvas.transform);
            foodSliders.Add(foodTimer.GetComponent<Slider>());
        }
        */

        foodTimerCanvasGroups = new List<CanvasGroup>();
        scaleSprings = new List<SpringToScale>();
        for (int i = 0; i < foodSliders.Count; i++)
        {
            foodTimerCanvasGroups.Add(foodSliders[i].GetComponent<CanvasGroup>());
            foodTimerCanvasGroups[i].alpha = 0.0f;
            scaleSprings.Add(foodSliders[i].GetComponent<SpringToScale>());
            //scaleSprings[i].SpringTo(Vector3.zero);
            foodTimerCanvasGroups[i].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }        
    }
    public void SetTimerVisible(int index, float alpha)
    {     
        foodTimerCanvasGroups[index].alpha = alpha;   
        scaleSprings[index].SpringTo(Vector3.one);
        //foodTimerCanvasGroups[index].alpha = alpha;        
    }    
    public void NudgeSpring(int index)
    {
        scaleSprings[index].Nudge(new Vector3(12, 4, 10));
    }
}
