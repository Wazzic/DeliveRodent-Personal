using System.Collections.Generic;
using UnityEngine;

public class PedestrianScript : MonoBehaviour
{
    
    private enum State
    {
        waitingForFood,
        walking,
        idle
    }
    [SerializeField] private State state;
    [SerializeField] private bool randomMesh;
    public Transform Character;

    private void Start()
    {
        Invoke("find", 5f);
        //Set all child objects to false, Starts at 1 because the first object is a VFX
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        // Set one random child as active
        if (randomMesh)
        {
            //stores the correct pedestrian character for the zone
            Character = transform.GetChild(Random.Range(1, transform.childCount));
            //Sets the object to active
            Character.gameObject.SetActive(true);
        }
    }

    //Used to make the pedestrian look at a transform
    public void PedestrianLookAtItem(Transform item)
    {
        transform.LookAt(item);
    }

}
