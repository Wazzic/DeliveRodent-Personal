using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficNodeData : MonoBehaviour
{
    public bool Stop;
    public bool whichDirection;
    public bool isCorner;
    public Vector3 getPos;
    public int whichNode;
    private void Awake()
    {
        getPos = transform.position;
    }

}
