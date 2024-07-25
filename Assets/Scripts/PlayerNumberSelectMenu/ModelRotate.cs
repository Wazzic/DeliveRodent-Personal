using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;

public class ModelRotate : MonoBehaviour
{
    SpringToScale scaleSpring;
    [SerializeField] Vector3 nudge;
    [SerializeField] float speed;
    public float time;
    [SerializeField] float rotationOffset;
    private void Awake()
    {
        time = 0.0f;
        //scaleSpring = GetComponent<SpringToScale>();
        //scaleSpring.Nudge(new Vector3(1000f, -700f, 1200f));
    }
    void Update()
    {
        time += 45 * speed * Time.deltaTime;
        if (time >= 360)
        {
            time = 0.0f;
            //scaleSpring.Nudge(nudge);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + time + rotationOffset, transform.rotation.z);
    }
}
