using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ArcadeVehicleController>() != null)
        {
            other.transform.position = target.position;
            other.transform.rotation = target.rotation;
        }
    }
}
