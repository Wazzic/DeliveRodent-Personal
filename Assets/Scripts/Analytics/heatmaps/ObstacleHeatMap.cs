using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class ObstacleHeatMap : MonoBehaviour
{
    /*
    public bool recordCollisions;

    ObstacleHeatMapData obstacleHeatMapData;
    public class CollisionData
    {
        public Vector3 collisionPosition;
        public string collisionType;
    }


    void Awake()
    {
        recordCollisions = false;
        //obstacleHeatMapData.collidedObstacle = new List<Vector3>();
    }
    public void StartCollisionRecording()
    {
        if (!recordCollisions)
        {
            recordCollisions = true;
            obstacleHeatMapData = ObstacleHeatMapData.CreateInstance<ObstacleHeatMapData>();

            string path = "Assets/Scripts/Analytics/Heatmaps/ObstacleHeatMapData/" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "collision.asset";
            AssetDatabase.CreateAsset(obstacleHeatMapData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Created obstacle collision data object at " + path);
            //EditorUtility.FocusProjectWindow();
            //obstacleHeatMapData.name = ;

            obstacleHeatMapData.position = new List<Vector3>();
            obstacleHeatMapData.collisionType = new List<string>();
        }
        else
        {
            Debug.Log("Collision data is already recording");
        }
        
    }
    public void RecordCollision(Vector3 collisionPos, string collisionType)
    {
        obstacleHeatMapData.position.Add(collisionPos);
        obstacleHeatMapData.collisionType.Add(collisionType);
    }

    */
}
