using Spring.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TargetIconHandler : MonoBehaviour
{
    [SerializeField] DeliveryVFXHandler vFXHandler;
    [SerializeField] MeshRenderer outlineMeshRenderer;
    [SerializeField] MeshRenderer iconMeshRenderer;
    [SerializeField] Material[] iconMaterials;

    [SerializeField] Camera camera;

    SpringToScale scaleSpring;
    SpringToRotation rotationSpring;

    int oldIconIndex;

    Transform car;
    Vector3 offset;

    private void Awake()
    {
        scaleSpring = GetComponent<SpringToScale>();
        rotationSpring = GetComponent<SpringToRotation>();

        car = transform.parent;
        offset = transform.localPosition;
        
        //offset.z *= 1.2f;
        //offset *= 0.5f;
        offset.x *= 0.2f;
        offset.z *= 0.2f;
        offset.y *= -0.05f;
        //this.transform.parent = null;
    }


    public void ChangeIcon(int iconIdex)
    {
        if (iconIdex > 2 || iconIdex < 0)
        {
            Debug.Log("new target icon is outside array");
            return;
        }
        iconMeshRenderer.material = iconMaterials[iconIdex];
        //ScaleNudge();
        if (iconIdex != oldIconIndex)
        {
            PlayEmojiEffect(iconIdex);
        }
        oldIconIndex = iconIdex;
    }

    public void ChangeIcon(Material mat)
    {
        if (!mat)
        {
            //Debug.Log("Icon Mat is NULL");
            return;
        }
        iconMeshRenderer.material = mat;
    }

    private void PlayEmojiEffect(int index)
    {
       
    }
    public void ChangeColour(Color targetColour)
    {
        outlineMeshRenderer.material.color = targetColour;
        //ScaleNudge();
    }
    public void ScaleNudge()
    {
        scaleSpring.Nudge(5 * Vector3.one);
    }
    
}
