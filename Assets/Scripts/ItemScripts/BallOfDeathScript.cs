using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallOfDeathScript : MonoBehaviour
{
    [SerializeField] AudioSource myAudioSource;
    public Collider PlayerCollider;
    private Collider myCollider;
    private MeshRenderer mesh;
    [SerializeField] LayerMask layerMask;
    float radius;
    [SerializeField] float downForce;
    [SerializeField] ParticleEffectGroup explosionParticleEffectGroup;
    [SerializeField] ParticleSystem sparks;
    Rigidbody rb;

    bool exploded;
    void Start()
    {
        radius = GetComponent<SphereCollider>().bounds.size.x / 2f;
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();

        exploded = false;

        //AnalyticsManager.instance.ballsFired++;

        StartCoroutine(WaitForCollisionWithPlayerThatSpawnedMe());

        Invoke("Explode", 3f);
    }
    
    IEnumerator WaitForCollisionWithPlayerThatSpawnedMe()
    {
        Physics.IgnoreCollision(PlayerCollider, myCollider, true);
        yield return new WaitForSeconds(1f);
        Physics.IgnoreCollision(PlayerCollider, myCollider, false);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * (radius + 1), Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, radius + 1, layerMask))
        {
            if (!(hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("PedestrianCar") || hit.collider.CompareTag("Player")))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + radius, hit.point.z);
            }
        }
        else
        {
            rb.AddForce(Vector3.down * downForce);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !exploded)
        {
            //rb.velocity = rb.velocity / 4f;
            //collision.rigidbody.velocity = GetComponent<Rigidbody>().velocity;
            //AnalyticsManager.instance.ballsHit++;
            Explode();
        }
    }
    void Explode()
    {
        exploded = true;

        Collider[] colliders = new Collider[4];
        int numberOfCollision;
        numberOfCollision = Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.z * 3f, colliders, 1 << 6);
        for (int i = 0; i < numberOfCollision; i++)
        {
            if (colliders[i].TryGetComponent<ArcadeVehicleController>(out ArcadeVehicleController arcadeVehicleController))
            {
                arcadeVehicleController.StunnedActionFunction(true, AngleDir(colliders[i].transform.position), 2f);
                colliders[i].attachedRigidbody.AddForce(Vector3.up * 25f);
            }
        }
        explosionParticleEffectGroup.Play();
        myAudioSource.Play();
        rb.isKinematic = true;
        myCollider.enabled = false;
        mesh.enabled = false;
        sparks.Stop();
        //destroy actual object after exploderizing it
        Destroy(gameObject,2f);
    }
    private Vector3 AngleDir(Vector3 playerPos)
    {
        Vector3 delta = (playerPos - transform.position).normalized;

        // Calculate the perpendicular vector to determine the direction of the angle
        Vector3 perp = Vector3.Cross(transform.forward, delta);

        // Calculate the dot product between the perpendicular vector and the up direction
        float dir = Vector3.Dot(perp, Vector3.up);

        // Check the direction of the angle
        if (dir > 0f)
        {
            // The angle is in the positive direction
            return transform.right;
        }
        else if (dir < 0f)
        {
            // The angle is in the negative direction
            return -transform.right;
        }
        else
        {
            // The vectors are perpendicular, the angle is neither positive nor negative
            return perp * -1; // shuv it left anyways
        }
    }
}
