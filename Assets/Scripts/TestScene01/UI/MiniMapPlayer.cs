using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayer : MiniMapObject
{
    // Update is called once per frame
     protected override void Update()
    {
        base.Update();       
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, followObject.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
