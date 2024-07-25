using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    [SerializeField] AudioSource myAudioSource;
    [SerializeField] float scaleUpDuration;
    [SerializeField] float activationDuration;
    [SerializeField] float upForce;
    [SerializeField] float downForce;
    bool isActive = false;
    //bool hasExploded;
    [HideInInspector]
    public Transform PointSpawnedFrom;
    Rigidbody rb;
    ParticleEffectGroup effectGroup;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        effectGroup = GetComponent<ParticleEffectGroup>();

        rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
        rb.AddTorque(GetRandomVector() * upForce, ForceMode.Impulse);

        // Ensure the object is set to the final scale
        //transform.localScale = targetScale;
        //AnalyticsManager.instance.minesFired++;

        StartCoroutine(WaitForActive());

        //StartCoroutine(ScaleUp(transform.localScale));
    }
    private Vector3 GetRandomVector()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        return new Vector3(randomX, randomY, randomZ);
    }
    private IEnumerator ScaleUp(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.zero;

        while (elapsedTime < scaleUpDuration)
        {
            float t = elapsedTime / scaleUpDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            transform.position = PointSpawnedFrom.position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);

        // Ensure the object is set to the final scale
        transform.localScale = targetScale;
       
        StartCoroutine(WaitForActive());

    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * downForce);
    }
    IEnumerator WaitForActive()
    {
        yield return new WaitForSeconds(activationDuration);

        isActive = true;
        
    }
    private void OnTriggerExit(Collider other)
    {
        isActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;
        // Hit something that isn't another rigidbody
        if (other.gameObject.GetComponent<Rigidbody>() == null)
        {
            rb.isKinematic = true;
            return;
        }
        // Did collide with a rigidbody
        //AnalyticsManager.instance.minesHit++;
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = new Collider[4];
        int numberOfCollision;
        numberOfCollision = Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.z * 3f, colliders, 1 << 6);
        for (int i = 0; i < numberOfCollision; i++)
        {
            if (colliders[i].TryGetComponent<ArcadeVehicleController>(out ArcadeVehicleController arcadeVehicleController))
            {
                arcadeVehicleController.StunnedActionFunction(true, Vector3.up, 1.5f);
            }
        }
        GetComponent<Collider>().enabled = false;
        effectGroup.Play();
        myAudioSource.Play();
        //hasExploded = true;
        Invoke("DestroySelf", 0.25f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void DestroySelf()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        //destroy actual object after exploderizing it        
        Destroy(gameObject, 1f);
    }

}
