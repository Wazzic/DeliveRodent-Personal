using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTimerWorldSpaceCanvas : MonoBehaviour
{
    public Camera playerCam;
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = playerCam;
    }
    private void LateUpdate()
    {
        if (playerCam != null)
        {
            transform.LookAt(transform.position + playerCam.transform.rotation * Vector3.forward,
                playerCam.transform.rotation * Vector3.up);

        }
    }
}
