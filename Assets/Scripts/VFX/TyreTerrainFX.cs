using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreTerrainFX : MonoBehaviour
{
    public string currentTerrain;

    public ArcadeVehicleController vehicleController;
    [SerializeField] private int wheelIndex;

    [SerializeField] string[] terrainTags;

    [SerializeField] ParticleEffectGroup[] terrainFX;
    [SerializeField] TrailRenderer[] terrainTrail;

    bool playEffect;
    public int terrainIndex;
    private void Start()
    {        
        vehicleController = transform.root.GetComponent<ArcadeVehicleController>();
    }

    private void Update()
    {
        if (vehicleController != null)
        {
            TogglePlayTerrainFX();                   
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        playEffect = false;
        currentTerrain = collider.gameObject.tag.ToString();
        for (int i = 0; i < terrainTags.Length; i++)
        {
            if (collider.gameObject.CompareTag(terrainTags[i]))
            {
                playEffect = true;
                terrainIndex = i;
                break;
                //TogglePlayTerrainFX(i);

                //terrainFX[i].Play();
                //terrainTrail[i].emitting = true;
            }
            /*
            else
            {
                playEffect = false;

                //terrainFX[i].Stop();
                //terrainTrail[i].emitting = false;
            }
            */
        }
    }
    private void TogglePlayTerrainFX()
    {
        if (vehicleController.AccelerationInput > 0 && playEffect)
        {
            terrainFX[terrainIndex].PlayContinous();
            terrainTrail[terrainIndex].emitting = true;
        }
        else
        {
            terrainFX[terrainIndex].Stop();
            terrainTrail[terrainIndex].emitting = false;

            /*
            for (int i = 0; i < terrainTags.Length; i++)
            {
                terrainFX[i].Stop();
                terrainTrail[i].emitting = false;
            }
            */
        }
    }

    /*
    private void CheckTerrainTag(RaycastHit terrainHit)
    {
        currentTerrain = terrainHit.transform.tag.ToString();
        for (int i = 0; i < terrainTags.Length; i++)
        {
            if (terrainHit.transform.CompareTag(terrainTags[i]))
            {
                terrainFX[i].Play();
                terrainTrail[i].emitting = true;
            }
            else
            {
                terrainFX[i].Stop();
                terrainTrail[i].emitting = false;
            }
        }
    }
    */
}
