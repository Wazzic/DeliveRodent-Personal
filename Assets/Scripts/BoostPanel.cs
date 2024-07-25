using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPanel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ArcadeVehicleController>(out ArcadeVehicleController arcadeVehicleController))
        {
            arcadeVehicleController.BoostPanelEnter();
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.TryGetComponent<ArcadeVehicleController>(out ArcadeVehicleController arcadeVehicleController))
    //    {
    //        arcadeVehicleController.BoostPanelStay();
    //    }
    //}
}
