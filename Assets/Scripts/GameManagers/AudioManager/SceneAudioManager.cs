using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    private List<ArcadeVehicleController> arcadeVehicleControllers = new List<ArcadeVehicleController>();

    [SerializeField] AudioSource engineAudioSource;
    [SerializeField] AudioSource skidAudioSource;
    [SerializeField] float minEnginePitch, maxEnginePitch;
    public void AddPlayerAudio(ArcadeVehicleController arcadeVehicleController)
    {
        arcadeVehicleControllers.Add(arcadeVehicleController);
    }
    private void Update()
    {
        if (arcadeVehicleControllers.Count == 0)
            return;
        DriftAudioCheck();
        PlayHighestPitchEngineSound();
    }
    private void PlayHighestPitchEngineSound()
    {
        float fastestCarVelocityZ = 0f;
        float maxPossibleSpeed = arcadeVehicleControllers[0].CurrentMaxSpeed;
        for (int i = 0; i < arcadeVehicleControllers.Count; i++)
        {
            float carVelocityZ = arcadeVehicleControllers[i].carRelativeVelocity.z;
            if (carVelocityZ > fastestCarVelocityZ)
            {
                fastestCarVelocityZ = carVelocityZ;
            }

        }
        // Pitch the engine sound depending on the cars velocity
        engineAudioSource.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, Mathf.Abs(fastestCarVelocityZ) / maxPossibleSpeed);
    }
    private void DriftAudioCheck()
    {
        skidAudioSource.mute = true;

        for (int i = 0; i < arcadeVehicleControllers.Count; i++)
        {
            if (arcadeVehicleControllers[i].LockedIntoDrift && arcadeVehicleControllers[i].IsGrounded)
            {
                skidAudioSource.mute = false;
            }
        }
    }
}
