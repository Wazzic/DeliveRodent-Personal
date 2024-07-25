using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapObject : MonoBehaviour
{
    [SerializeField]
    protected GameObject followObject;

    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (followObject.activeSelf)
        {
            transform.position = followObject.transform.position;
            SpriteRenderer.color = Color.white;
        }
        else
        {
            SpriteRenderer.color = Color.clear;
        }
    }

    public GameObject GetFollowObject()
    {
        return followObject;
    }
    public void SetFollowObject(GameObject obj)
    {
        followObject = obj;
    }
}
