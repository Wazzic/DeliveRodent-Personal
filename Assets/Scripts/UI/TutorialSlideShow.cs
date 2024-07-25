using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialSlideShow : MonoBehaviour
{
    public float time = 0.0f;
    [SerializeField] private float changeTime;

    private int panelIndex = 0;
    private List<CanvasGroup> panels = new List<CanvasGroup>();

    [SerializeField] List<VideoClip> clips;
    [SerializeField] VideoPlayer videoPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        panels = GetComponentsInChildren<CanvasGroup>().ToList();
        foreach(CanvasGroup grp in panels)
        {
            grp.alpha = 0.0f;
        }
        panels[panelIndex].alpha = 1.0f;
        videoPlayer.clip = clips[0];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > changeTime)
        {
            NextPanel();
            time = 0.0f;
        }
    }
    public void NextPanel()
    {
        panels[panelIndex].alpha = 0.0f;

        panelIndex++;
        if (panelIndex > panels.Count - 1)
        {
            panelIndex = 0;
        }

        panels[panelIndex].alpha = 1.0f;
        videoPlayer.clip = clips[panelIndex];
    }
    public void PrevPanel()
    {
        panels[panelIndex].alpha = 0.0f;

        panelIndex--;
        if (panelIndex < 0)
        {
            panelIndex = panels.Count - 1;
        }
        panels[panelIndex].alpha = 1.0f;
        videoPlayer.clip = clips[panelIndex];
    }
}
