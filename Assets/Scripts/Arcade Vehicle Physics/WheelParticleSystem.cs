using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleSystem : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] skidMarks;
    [SerializeField] private ParticleSystem[] smokes;
    [SerializeField] private ParticleSystem[] sparks1;
    [SerializeField] private ParticleSystem[] sparks2;
    [SerializeField] private ParticleSystem[] sparks3;
    private ArcadeVehicleController carController;
    float fadeOutSpeed;
    Color brown;

    [SerializeField] private string[] materialTags;

    [SerializeField] private ParticleSystem[] grassTerrainEffects;
    [SerializeField] private ParticleSystem[] beachTerrainEffects;

    private void Awake()
    {
        for(int i = 0; i < skidMarks.Length; i++)
        {
            SetSkidMarksEmitting(i, false);            
            //Assuming the number of wheel is the same as the amount of skid marks
        }
    }
    private void Start()
    {
        brown = new Color32(165, 42, 42, 255);
        carController = transform.root.GetComponent<ArcadeVehicleController>();
    }

    private void FixedUpdate()
    {
        if (!carController) return;

        SkidMarkTrails();
        //DriftParticals();
        //TyreSmoke();
        SparksBurst();        
    }
    
    private void SkidMarkTrails()
    {
        for (int i = 0; i <4; i++)
        {
            if (carController.WheelIsGrounded[i])
            {
                if (Mathf.Abs(carController.carRelativeVelocity.x) > 15 || carController.HandBrakeInput)
                {
                    //skidMarks[i].materials[0].color = Color.black;
                    SetSkidMarksEmitting(i, true);
                }
                else
                {
                    SetSkidMarksEmitting(i, false);
                }
                //if (carController.IsOffRoading)
                //{
                //    skidMarks[i].materials[0].color = brown;
                //    SetSkidMarksEmitting(i, true);
                //}
            }
            else // If wheel is not grounded, disable
            {
                SetSkidMarksEmitting(i, false);
            }
        }
    }
    /*
    private void DriftParticals()
    {
        for (int i = 0; i < 2; i++)        
        {
            // If current boost level is 0 or if car is in air, stop playing smoke effect
            if (carController.CurrentBoostLevel == 0 || !carController.IsGrounded)
            {
                smokes[i].Stop();
                continue;
            }
            var main = smokes[i].main;
            // drift sparks
            if (carController.CurrentBoostLevel == 1)
            {
                //main.startColor = Color.yellow;
            }
            if (carController.CurrentBoostLevel == 2)
            {
                //main.startColor = Color.red;
            }
            if (carController.CurrentBoostLevel == 3)
            {
                //main.startColor = Color.magenta;
            }
            smokes[i].Play();

        }
    }
    */
    private void TyreSmoke()
    {
        foreach (ParticleSystem smoke in smokes)
        {
            if (!carController.LockedIntoDrift || !carController.IsGrounded)
            {
                smoke.Stop();
                continue;
            }
            /*
            else if (carController.CurrentBoostLevel == 1)
            {

            }
            else if (carController.CurrentBoostLevel == 2)
            {

            }
            else if (carController.CurrentBoostLevel == 3)
                //Final boost level
            {

            }
            */
            smoke.Play();
        }    
    }  
    private void SparksBurst()
    {
        for (int i = 0; i < 2; i++)
        {
            if (carController.CurrentBoostLevel == 0 || !carController.IsGrounded)
            {
                sparks1[i].Stop();
                sparks2[i].Stop();
                sparks3[i].Stop();
                continue;
            }
            if (carController.CurrentBoostLevel == 1)
            {
                sparks1[i].Play();
            }
            else if (carController.CurrentBoostLevel == 2)
            {
                sparks2[i].Play();
            }
            else if (carController.CurrentBoostLevel == 3)
            {
                sparks3[i].Play();
            }
        }
    }
            
    void SetSkidMarksEmitting(int i, bool b)
    {
        skidMarks[i].emitting = b;
    }
}
