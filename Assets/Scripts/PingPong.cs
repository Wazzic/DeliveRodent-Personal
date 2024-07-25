using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    public enum ObjectType
    {
        yMove,
        scale
    }
    public ObjectType type;
    [SerializeField] float speed = 1f;     // Speed of the movement
    [SerializeField] float height = 2f;    // Total height of the movement

    private Vector3 startPosition;
    private Vector3 startScale;
    private float timer = 0f;

    private void Start()
    {
        startPosition = transform.localPosition;
        startScale = transform.localScale;
    }

    private void Update()
    {
        if(type == ObjectType.yMove)
        {
            timer += Time.deltaTime * speed;
            float newY = Mathf.Lerp(startPosition.y, startPosition.y + height, Mathf.PingPong(timer, 1f));
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        if (type == ObjectType.scale)
        {
            timer += Time.deltaTime * speed;
            float newScale = Mathf.Lerp(startScale.y, startScale.y + height, Mathf.PingPong(timer, 1f));
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}
