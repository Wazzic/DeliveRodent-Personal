using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayLoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private float wait;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        rawImage.enabled = false;
        text.SetActive(false);
        StartCoroutine(PlayVideo());
        //videoPlayer
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        yield return new WaitForSeconds(wait);
        while (!videoPlayer.isPrepared)
        {
            yield return wait;
            break;
        }
        rawImage.enabled = true;
        text.SetActive(true);
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        yield return new WaitForSeconds(wait);
        //videoPlayer.Stop();
        //rawImage.texture = null;

    }
}
