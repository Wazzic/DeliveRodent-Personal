using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehind : MonoBehaviour
{
    Transform player;
    [SerializeField] Transform target;
    [SerializeField] GameObject icon;
    [SerializeField] Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;
        if (!target) return;

        Vector3 direction = (target.position - player.position).normalized;
        
        float  angle = AngleTo(new Vector2(camera.transform.forward.x, camera.transform.forward.z).normalized, new Vector2(direction.x, direction.z).normalized);
        angle = Mathf.Deg2Rad * angle * 2f;
        Vector2 dir =  new Vector2(-Mathf.Cos(angle) * Vector2.up.x - Mathf.Sin(angle) * Vector2.up.y, Mathf.Sin(angle) * Vector2.up.x + Mathf.Cos(angle) * Vector2.up.y);



        icon.transform.GetComponent<RectTransform>().transform.localPosition = new Vector3(dir.x * 100, dir.y * 100, 0);
    }

    private float AngleTo(Vector2 this_, Vector2 to)
    {
        Vector2 direction = to - this_;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }

}
