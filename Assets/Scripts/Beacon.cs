using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField] private Material hasItem;
    [SerializeField] private Material noItem;
    [SerializeField] private MeshRenderer[] beaconmeshes;
    Delivery delivery;
    private void Start()
    {
        delivery = transform.root.GetComponent<Delivery>();
    }

    private void Update()
    {
        for (int i = 0; i < beaconmeshes.Length; i++)
        {
            if (delivery.PlayerHasItem())
            {
                beaconmeshes[i].material = hasItem;
            }
            else
            {
                beaconmeshes[i].material = noItem;
            }
        }
       
      
    }
}
