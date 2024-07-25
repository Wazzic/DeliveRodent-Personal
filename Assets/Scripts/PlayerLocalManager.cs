using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLocalManager : MonoBehaviour
{
    public int ID;

    [SerializeField] Camera myCamera;
    [SerializeField] GameObject myFollowCamera;
    [SerializeField] Material mat1, mat2;
    [SerializeField] Renderer bodyRenderer;

    public int totalPlayers;
    public bool hasDelivery;
    bool invicible;

    [SerializeField] GameObject attackCollider;
    private void Start()
    {
        
        //SetCameraCullingMask();
        //SetupMaterial();
        gameObject.name = "Player" + ID;
        myCamera.name = "Player" + ID + " Camera";
        // myCamera.transform.parent = null;
        if (totalPlayers == 1)
        {
            myCamera.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
        }
        if (totalPlayers == 2)
        {
            if (ID == 0)
            {
                myCamera.rect = new Rect(new Vector2(-0.5f, 0), new Vector2(1, 1));
            }
            if (ID == 1)
            {
                myCamera.rect = new Rect(new Vector2(0.5f, 0), new Vector2(1, 1));
            }
        }



        if (FindObjectsOfType<AudioListener>().Length > 0)
        {
            //Destroy(FindObjectOfType<AudioListener>().GetComponent<AudioListener>());
        }
    }
    void SetCameraCullingMask()
    {
        //set layername of our follow camera to playercamera plus our unique ID
        myFollowCamera.gameObject.layer = LayerMask.NameToLayer("PlayerCamera" + ID.ToString());
        //cycle through 4 potenial cameras
        for (int i = 0; i < 4; i++)
        {
            // cull player camera masks that aren't our own
            if (i != ID) 
            {
                int layerMaskToCull = LayerMask.NameToLayer("PlayerCamera" + i.ToString());
                myCamera.cullingMask &= ~(1 << layerMaskToCull);
            }
        }
    }
    void SetupMaterial()
    {
        if (ID == 0)
        {
            bodyRenderer.material = mat1;
        }
        if (ID == 1)
        {
            bodyRenderer.material = mat2;
        }
    }
    
    
}
