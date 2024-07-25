using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring.Runtime;
using UnityEngine.VFX;
using DG.Tweening;

public class ObstacleManager : MonoBehaviour
{
    public enum ObjectType
    {
        destructable,
        moveable,
        springy,
        unmovable
    }
    public ObjectType type;

    [SerializeField] private float otherObjectVelocityMultipier;

    [Header("Movealbe")]
    [SerializeField] private float downForce;
    [SerializeField] private float velocityMultipier;
    [SerializeField] private bool shrinkAfterGettingHit;

    [Header("Springy")]
    [SerializeField] private bool overrideNudgeStrength;
    [SerializeField] Vector3 nudgeStrength;

    [Header("Unmoveable")]


    private bool hasCollided = false;
    private Rigidbody rb;
    private SpringToScale scaleSpring;
    private ParticleEffectGroup particleGroup;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        if (type == ObjectType.springy)
        {
            scaleSpring = GetComponent<SpringToScale>();
        }
        if (GetComponentInChildren<ParticleEffectGroup>() != null)
        {
            particleGroup = GetComponentInChildren<ParticleEffectGroup>();
        }
        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody otherRigidbody))
        {
            if (otherRigidbody.isKinematic)
            {
                return;
            }
        }
        else if (other.transform.root.TryGetComponent(out PedestrianScriptCarReal pedestrianCarScript)) // the pedestrian cars have collider on a child and rigidbody on the parent
        {
            if (pedestrianCarScript.GetIsHit() == false)
            {
                return;
            }
        }
        else
        {
            return;
        }
        audioSource.Play();

        switch (type)
        {
            case ObjectType.destructable:
                if (particleGroup != null)
                {
                    particleGroup.Play();
                }
                //transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InSine).onComplete = DestroyObject;
                DestroyObject();
                if (other.CompareTag("Player"))
                {
                    Rigidbody hitPlayerRB = other.GetComponent<Rigidbody>();
                    hitPlayerRB.velocity *= otherObjectVelocityMultipier;
                    

                    //other.GetComponent<PlayerItemsHandler>().ReduceItemCD();
                }
                break;
            case ObjectType.springy:
                if (overrideNudgeStrength)
                {
                    scaleSpring.Nudge(nudgeStrength);
                }
                else
                {
                    scaleSpring.Nudge(new Vector3(1000.0f, 0f, 1000.0f));
                }
                break;
            case ObjectType.moveable:
                if(shrinkAfterGettingHit)
                {
                    StartCoroutine(ShrinkAndDestroyObject());
                }

                //Apply force relative to the onject that collided with it
                rb.isKinematic = false;
                //rb.velocity = collision.relativeVelocity * 2f;
                hasCollided = true;
                StartCoroutine(downwardsForce());
                GetComponent<Collider>().isTrigger = false;
                if (other.CompareTag("Player"))
                {
                    Rigidbody playerRB = other.GetComponent<Rigidbody>();
                    //rb.velocity = playerRB.velocity * velocityMultipier;
                    playerRB.velocity = playerRB.velocity * otherObjectVelocityMultipier;

                }
                foreach (var col in GetComponentsInChildren<Collider>())
                {
                    col.isTrigger = false;
                }
                
                break;

        }
    }

    private void DestroyObject()
    {
        //Instantiate(vfxPrefab, gameObject.transform.position, Quaternion.identity);
        //vfxPrefab.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
    private IEnumerator ShrinkAndDestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0.0f;
        float duration = 0.5f;
        while (elapsedTime < duration)
        {
            // Calculate the new scale based on the lerp factor
            float lerpFactor = elapsedTime / duration;
            Vector3 newScale = Vector3.Lerp(startScale, Vector3.zero, lerpFactor);

            // Apply the new scale to the object
            transform.localScale = newScale;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches scale 0 exactly
        transform.localScale = Vector3.zero;

        // Destroy the object
        Destroy(transform.gameObject);
    }

    IEnumerator downwardsForce()
    {
        while (hasCollided)
        {
            rb.AddForce(Vector3.down * downForce, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }

        
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (hasCollided)
    //    {
    //        rb.AddForce(Vector3.down * downForce, ForceMode.VelocityChange);
    //    }
    //}
}
