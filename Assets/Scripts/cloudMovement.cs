using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMovement : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0.0f, 0.0f, 10.0f * Time.deltaTime);
        if(transform.position.z > 5900f)
        {
            transform.position = new Vector3(0.0f, 400.0f, -5900f);
        }
    }
}
