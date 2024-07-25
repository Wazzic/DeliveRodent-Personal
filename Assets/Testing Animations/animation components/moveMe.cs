using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;

public class moveMe : MonoBehaviour
{
    [SerializeField] bool isJogging;
    [SerializeField] GameObject route;
    [SerializeField] int currIndex;

    [SerializeField]List<Transform> destination;
    float m_speed;
    float m_wayPointOffSet = 5.0f;
    float m_angleOffset = 0.75f;
    float newRot = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!isJogging)
        {
            m_speed = 4.0f;
        }
        else
        {
            m_speed = 8f;
        }

        foreach(Transform t in route.GetComponentsInChildren<Transform>()) 
        {
            destination.Add(t);
        }

        destination.RemoveAt(0);
    }

    private void walk()
    {
        transform.position += transform.forward * m_speed * Time.deltaTime;
    }
    
    private void turn()
    {
        Vector3 targetDir = destination[currIndex].transform.position - transform.position;
        float singleStep = m_angleOffset * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);
        
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void checkNodes()
    {
        if(currIndex == destination.Count - 1)
        {
            currIndex = 0;
        }

        if (Vector3.Distance(transform.position, destination[currIndex].transform.position) < m_wayPointOffSet)
        {
            currIndex++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        walk();
        turn();
        checkNodes();
    }
}
