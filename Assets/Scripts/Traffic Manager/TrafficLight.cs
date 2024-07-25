using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] List<TrafficNodeData> connectedNodes; 
    [SerializeField] bool m_hasStopped;
    Material m_material;
    [SerializeField] float m_timer;
    public bool getStatus()
    {
         return m_hasStopped; 
    }
    // Start is called before the first frame update
    void Start()
    {
        m_timer = 15;
        m_hasStopped = true;
        m_material = GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedNodes.Count>0)
        {

            if (m_timer > 0)
            {
                m_timer -= Time.deltaTime;
            }
            else
            {
                m_timer = 5;
                m_hasStopped = !m_hasStopped;

                connectedNodes[0].Stop = m_hasStopped;
                             
            }
            if (m_hasStopped)
            {
                m_material.color = Color.red;
            }
            else
            {
                m_material.color = Color.green;
            }

        }
    }
}
