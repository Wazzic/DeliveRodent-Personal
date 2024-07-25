using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject forwardCamera, lookBackCamera;

    [SerializeField] public CinemachineVirtualCamera followCam;
    [SerializeField] AnimationCurve FOVCurve;
    [SerializeField] AnimationCurve ZFollowCurve;
    [SerializeField] float fovEffectDuration;
    private float elapsedTime = 0.0f; 
    float valueFrom0To1;
    bool isInEffect;
    bool isHoldingLookBackButton;


    public void OnLookbackButton(InputAction.CallbackContext context)
    {
        if (InputManager.instance.controlsEnabled)
        {
            isHoldingLookBackButton = context.action.WasPressedThisFrame();
        }
    }
    private void Start()
    {
        isHoldingLookBackButton = false;
    }
    public void Update()
    {
        UpdateSelectedCamera();
    }

    private void UpdateSelectedCamera()
    {
        if (isHoldingLookBackButton)
        {
            lookBackCamera.SetActive(true);
            forwardCamera.SetActive(false);
        }
        else
        {
            lookBackCamera.SetActive(false);
            forwardCamera.SetActive(true);
        }
    }

    IEnumerator Dolly()
    {
        isInEffect = true;
        // valueFrom0To1 will go from 0 to 1 over "duration" seconds
        elapsedTime = 0;
        while (elapsedTime < fovEffectDuration)
        {
            elapsedTime += Time.deltaTime;
            valueFrom0To1 = Mathf.Lerp(0, 1, elapsedTime / fovEffectDuration);

            followCam.m_Lens.FieldOfView = FOVCurve.Evaluate(valueFrom0To1);

            var transposer = followCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = new Vector3(0, transposer.m_FollowOffset.y, ZFollowCurve.Evaluate(valueFrom0To1));

            yield return null;
        }
        isInEffect = false;

    }
    public void IncreaseFieldOfView()
    {
        if (!isInEffect)
        {
            StartCoroutine(Dolly());
        }


    }
}
