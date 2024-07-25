using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CarSettings : ScriptableObject
{
    public float defaultMaxSpeed = 50;
    public float CurrentMaxSpeed = 50;
    public float CurrentAcceleration = 15f;
    public float brake = 15; //same as currentAcceleration
    private float turnMultiplier = 30;
    //private float torqueMultiplier = 1f;
    public AnimationCurve frictionCurve;
    public AnimationCurve turnCurve;
    public AnimationCurve driftInputCurve;
    public class DrivingSettings
    {
        
    }
    public float driftTimeTillBoost = 1;
    public float driftSpeedBonus = 2;
    public float driftAccelerationBonus = 25;
    public float driftBoostTime = 0.5f;
    public float driftSidewaysForce = 100;
    public class DriftSettings
    {
        
    }        

}
