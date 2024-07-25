using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasedOnSrufacesTest : MonoBehaviour
{
    [SerializeField] Transform[] startPoints;
    List<Vector3> groundedWheels = new List<Vector3>();
    List<Vector3> hitPoints = new List<Vector3>();
    RaycastHit hit;
    void Update()
    {
        hitPoints.Clear();
        groundedWheels.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(startPoints[i].position, -transform.up, out hit, 2))
            {
                hitPoints.Add(hit.point);
                groundedWheels.Add(startPoints[i].position);
            }
        }

        Vector3 totalHits = Vector3.zero;
        Vector3 totalWheels = Vector3.zero;
        for (int i = 0; i < hitPoints.Count; i++)
        {
            totalHits += hitPoints[i];
            totalWheels += groundedWheels[i];
        }
        totalHits /= hitPoints.Count;
        totalWheels /= groundedWheels.Count;

        transform.rotation = Quaternion.FromToRotation(transform.up, totalHits - totalWheels);
    }
}
