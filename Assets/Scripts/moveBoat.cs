using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBoat : MonoBehaviour
{
    [SerializeField] float m_speed;
    // Update is called oTM_SPEEDnce per frame
    void Update()
    {
        gameObject.transform.position -= gameObject.transform.right * (m_speed*  Time.deltaTime);
    }
}
