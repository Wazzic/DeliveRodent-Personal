using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftMode : MonoBehaviour
{
    KartController01 kartController;
    void Awake()
    {
        kartController = GetComponent<KartController01>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
        }
    }
}
