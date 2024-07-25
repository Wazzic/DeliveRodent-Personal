using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryVFXHandler : MonoBehaviour
{
    public List<ParticleEffectGroup> particleEffects;
    public List<DeliverySpring> deliverySprings;
    public SmellTrailController SmellTrail;

    StunEffect stunEffect;
    ParticleEffectGroup boost;
    ShieldEffect shieldEffect;

    private void Awake()
    {
        particleEffects = new List<ParticleEffectGroup>();
        particleEffects = GetComponentsInChildren<ParticleEffectGroup>().ToList();        

        stunEffect = GetComponentInChildren<StunEffect>();

        shieldEffect = GetComponentInChildren<ShieldEffect>();
    }
    private void Start()
    {
       boost = transform.root.GetComponentInChildren<CarVisualsHandler>().Nitrous;

       deliverySprings = transform.root.GetComponentsInChildren<DeliverySpring>(true).ToList();
    }
    public void PlayAttackVFX()
    {
        particleEffects[0].Play();
    }
    public void PlayStealVFX()
    {
        particleEffects[1].Play();
    }
    public void PlayStolenVFX()
    {
        particleEffects[2].Play();
    }
    public void PlaySpeedLines()
    {
        particleEffects[3].Play();
    }
    public void StopSpeedLines()
    {
        particleEffects[3].Stop();
    }
    public void PlayPickUpVFX()
    {
        particleEffects[4].Play();
    }
    public void PlayDropOffVFX()
    {
        particleEffects[5].Play();
    }
    public void PlayAngryEmoji()
    {
        particleEffects[6].Play();
    }
    public void PlaySurprisedEmoji()
    {
        particleEffects[6].Play();
    }
    public void PlayStunEffect()
    {
        stunEffect.PlayStunEffect();
    }
    public void PlayShieldEffect()
    {
        particleEffects[9].Play();
        shieldEffect.PlayShield();
    }

    

    public void NudgeDeliveryScaleSpring(int index)
    {
        deliverySprings[index].NudgeDeliveryScale();
    }
   
    public void PlayBoostExhaustFlame()
    {
        //boostExhaustFlame.enabled = true;
        boost.Play();
    }
    public void StopBoostExhaustFlame()
    {
        //boostExhaustFlame.enabled = false;
        boost.Stop();
    }
    
}
