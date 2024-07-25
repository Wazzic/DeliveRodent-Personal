using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCreator : MonoBehaviour
{
    [HideInInspector]
    public Track track;
    private List<GameObject> segments;
    [SerializeField] GameObject roadSegment;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;

    public float controlDiameter = .075f;

    public bool displayControlPoints = false;

    public void CreateTrack()
    {
        track = new Track(transform.position);
    }

    void Reset()
    {
        CreateTrack();
    }

    void Awake()
    {
        segments = new List<GameObject>();
    }

    public GameObject InstSegment(Vector3 pos)
    {

        GameObject newSegment = Instantiate(roadSegment, pos, Quaternion.identity);

        //newSegment.transform.parent = transform;        

        segments.Add(newSegment);

        //newSegment.transform.SetParent(this.transform.parent);

        return newSegment;



    }

    public void PosMesh(Transform segment, int loopIndex)
    {
        Transform[] controlTransforms = segment.GetComponentsInChildren<Transform>();
        Vector3[] controlPoints = track.GetPointsInSegment(loopIndex);

        for (int i = 1; i < segment.transform.childCount; i++)
        {
            for (int j = 0; j < controlPoints.Length; j++)
            {
                controlTransforms[j].position = controlPoints[j];

                Debug.Log(controlTransforms[j]);
            }
            controlTransforms[3].position = controlPoints[3];

        }

        /*
        Transform[] transforms = new Transform[4];
        for (int i = 0; i < track.NumSegments; i++)
        {
            Vector3[] cp = track.GetPointsInSegment(i);
            segments[i] = InstSegment(cp[1]);
            for (int j = 0; j < segments.Count - 1; j++)
            {
                //segments[j]
            }
            transforms[0] = this.gameObject.transform.GetChild(0);
            transforms[1] = this.gameObject.transform.GetChild(1);
            transforms[2] = this.gameObject.transform.GetChild(2);
            transforms[3] = this.gameObject.transform.GetChild(3);
            for (int j = 0; j < 4; j++)
            {
                transforms[j].transform.position = cp[j];
            }            
            
        }
        */

    }
}