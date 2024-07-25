using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackCreator))]
public class TrackEditor : Editor
{

    TrackCreator creator;

    Track Track
    {
        get
        {
            return creator.track;
        }
    }

    int selectedSegmentIndex = 1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreateTrack();
        }

        bool isClosed = GUILayout.Toggle(Track.IsClosed, "Toggle Closed");
        if (isClosed != Track.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            Track.IsClosed = isClosed;
        }

        if (GUILayout.Button("Add Segment"))
        {
            Undo.RecordObject(creator, "Add Segment");
            Vector3 newSegPos = Vector3.zero;
            newSegPos = (Track[Track.NumPoints - 1]) - (0.5f * (Track[Track.NumPoints - 4]));

            Track.AddSegment(newSegPos);
            selectedSegmentIndex++;
        }

        if (GUILayout.Button("Generate Mesh"))
        {
            Undo.RecordObject(creator, "Generate Mesh");
            if (Application.isPlaying)
            {
                for (int i = 0; i < Track.NumSegments; i++)
                {
                    GameObject newSegPos = creator.InstSegment(Track.points[0] + new Vector3(0, i, 0));

                    newSegPos.transform.parent = creator.transform;

                    creator.PosMesh(creator.gameObject.transform.GetChild(i), i);

                }
            }
            else
            {
                Debug.Log("Cannot GenMesh In Editor");  //Note this is pretty shitty. Maybe refactor so as to not do this?
            }

        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        //Input();
        Draw();
    }

    /*
    void Input()
    {
        Event guiEvent = Event.current;
        //Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split segment");
                Track.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if (!Track.IsClosed)
            {
                Undo.RecordObject(creator, "Add segment");
                Track.AddSegment(mousePos);
            }
        }
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = creator.anchorDiameter * .5f;
            int closestAnchorIndex = -1;
            for (int i = 0; i < Track.NumPoints; i+=3)
            {
                float dst = Vector2.Distance(mousePos, Track[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }
            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete segment");
                Track.DeleteSegment(closestAnchorIndex);
            }
        }
        
        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;
            for (int i = 0; i < Track.NumSegments; i++)
            {
                Vector2[] points = Track.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }
            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        } 
        HandleUtility.AddDefaultControl(0);
    }
    */

    void Draw()
    {
        for (int i = 0; i < Track.NumSegments; i++)
        {
            Vector3[] points = Track.GetPointsInSegment(i);
            Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentCol : creator.segmentCol;

            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2f);

            if (creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
        }


        int step;
        if (!Event.current.shift)
        {
            step = 1;
        }
        else
        {
            step = 3;
            creator.displayControlPoints = false;
        }



        for (int i = 0; i < Track.NumPoints; i = i + step)
        {
            Vector3 newPos = Handles.PositionHandle(Track[i], Quaternion.identity);
            if (Track[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");
                Track.MovePoint(i, newPos);
            }
            //}
        }
    }

    void OnEnable()
    {
        creator = (TrackCreator)target;
        if (creator.track == null)
        {
            creator.CreateTrack();
        }
    }
}
