using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] PlayerPreFab;
    [SerializeField]
    private GameObject DropOffPrefab;
    [SerializeField]
    private GameObject PickUpPrefab;

    private bool foundOb;

    //private void Awake()
    //{

    //    GameObject[] pickUpPoints = GameObject.FindGameObjectsWithTag("PickUpPoint");
    //    GameObject[] dropOffPoints = GameObject.FindGameObjectsWithTag("DropOffPoint");

    //    for (int i = 0; i < pickUpPoints.Length; i++)
    //    {
    //        SpawnMiniMapObject(pickUpPoints[i], PickUpPrefab);
    //    }
    //    for (int i = 0; i < dropOffPoints.Length; i++)
    //    {
    //        SpawnMiniMapObject(dropOffPoints[i], DropOffPrefab);
    //    }

    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    foundOb = false;     

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    ////Temporary way of finding the player will change later on
    //    //if (!foundOb)
    //    //{
    //    //    for (int i = 0; i < 4; i++)
    //    //    {
    //    //        GameObject obj = GameObject.Find("Player" + i);
    //    //        if (obj)
    //    //        {
    //    //            foundOb = true;
    //    //            SpawnMiniMapObject(obj, PlayerPreFab[i]);
    //    //        }
    //    //    }
    //    //}
    //}

    ////void SpawnPlayerMiniMapObject(GameObject obj)
    ////{
    ////    GameObject miniMapObject = Instantiate(PlayerPreFab, this.transform);
    ////    miniMapObject.GetComponent<MiniMapPlayer>().SetFollowObject(obj);
    ////}

    //Will be used to assign minimap object to a object in sceme
    void SpawnMiniMapObject(GameObject obj, GameObject prefab)
    {      
        //GameObject miniMapObject = Instantiate(prefab, this.transform);
        //miniMapObject.GetComponent<MiniMapObject>().SetFollowObject(obj);
    }
}
