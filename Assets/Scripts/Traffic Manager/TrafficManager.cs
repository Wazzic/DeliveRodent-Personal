using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TrafficNodes
{
    public Vector3 destination;    //Location car will move to
    public bool hasTrafficLight;   //Location of node to stop at if traffic light is attached
    public bool directionOfTraffic;//Determines the flow of traffic.
    public bool isCorner;
}

[System.Serializable]
public struct TrafficRoute {
    public List<TrafficNodeData> childNodes;
    public TrafficRoute(List<TrafficNodeData> nodes)
    {
        childNodes = nodes;
    }
}
public class TrafficManager : MonoBehaviour
{
    [SerializeField] private GameObject pedestrianCar;
    [SerializeField] private int amountOfCars;
    [SerializeField] private int amountOfCarsPerRoute;

    public Color[] lineColor;

    //Get the container that holds the data
    [SerializeField] Component[] m_parentList;


    [SerializeField] public List<TrafficRoute> m_trafficRoutes;

    //Used container for all traffic data
    [SerializeField] List<TrafficNodes> m_TrafficContainer;
    //Extract the passed in traffic light 
    [SerializeField] TrafficLight m_lightStatus;

    [SerializeField] bool m_reachedLocation;

    [SerializeField] int m_stopPoint;

    

    //Use for singleton to be called in any class
    public static TrafficManager instance;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < m_trafficRoutes.Count; i++)
        {
            Gizmos.color = lineColor[i];
            for (int j = 0; j < m_trafficRoutes[i].childNodes.Count; j++)
            {
                Vector3 currNode = m_trafficRoutes[i].childNodes[j].getPos;
                Vector3 nextNode = Vector3.zero;

                if(j < m_trafficRoutes[i].childNodes.Count -1)
                {
                    nextNode = m_trafficRoutes[i].childNodes[j + 1].getPos;
                }
                else
                {
                    nextNode = m_trafficRoutes[i].childNodes[0].getPos;
                }
                Gizmos.DrawLine(currNode, nextNode);
            }
        }
    }
    private void Awake()
    {
        //Makes sure there is only one traffic manager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        lineColor = new Color[m_parentList.Length];

        #region set up a container for the nodes
        //Adds multiple routes to the traffic routes variable
        for (int i = 0; i < m_parentList.Length; i++)
        {
            m_trafficRoutes.Add(new TrafficRoute(new List<TrafficNodeData>(m_parentList[i].GetComponentsInChildren<TrafficNodeData>())));
            lineColor[i] = new Color(Random.value, Random.value, Random.value, 1.0f);
        }
        #endregion

      

    }

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < m_trafficRoutes.Count; i++)
        {
            for(int j = 0; j < amountOfCarsPerRoute; j++)
            {
                int currentNode = (int)Mathf.Clamp((float)m_trafficRoutes[i].childNodes.Count / amountOfCarsPerRoute * j, 0, m_trafficRoutes[i].childNodes.Count-1);
                Vector3 startPos = m_trafficRoutes[i].childNodes[currentNode].getPos;
                Vector3 offset = new Vector3(0, 7, 0);
                GameObject newCar = Instantiate(pedestrianCar, startPos + offset, Quaternion.identity);
                newCar.GetComponent<PedestrianScriptCarReal>().SetRoute(m_trafficRoutes[i]);
                newCar.GetComponent<PedestrianScriptCarReal>().SetNode(currentNode);
                amountOfCars++;
            }
        }

        //for (int i = 0; i < amountOfCars; i++)
        //{
        //    int randTrafficRoute = Random.Range(0, 4); ;
        //    //if( i % 3 == 0)
        //    //{
        //    //    randTrafficRoute = 4;
        //    //}
        //    //else if(i % 10 == 0)
        //    //{
        //    //    randTrafficRoute = 0;
        //    //}
        //    //else
        //    //{
        //    //    randTrafficRoute = Random.Range(1, m_trafficRoutes.Count - 1);
        //    //}
        //    int randNode = Random.Range(0, m_trafficRoutes[randTrafficRoute].childNodes.Count - 1);
        //    Vector3 startPos = m_trafficRoutes[randTrafficRoute].childNodes[randNode].getPos;
        //    Vector3 offset = new Vector3(0, 7, 0);
        //    GameObject newCar = Instantiate(pedestrianCar, startPos + offset, Quaternion.identity);
        //    newCar.GetComponent<PedestrianScriptCarReal>().SetRoute(m_trafficRoutes[randTrafficRoute]);
        //    newCar.GetComponent<PedestrianScriptCarReal>().SetNode(randNode);
        //}
        
    }

}
