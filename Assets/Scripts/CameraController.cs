using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 viewPortSize;

    public GameObject target;
    public float viewPortFactor;
    public float followDuration;
    public float maxFollowSpd;

    Camera cam;
    private Vector3 targetPos;
    private Vector3 curVel;

    Vector2 distance;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        targetPos = target.transform.position;
    }

    void FixedUpdate()
    {
        viewPortSize = (cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - cam.ScreenToWorldPoint(Vector2.zero)) * viewPortFactor;

        distance = target.transform.position - transform.position;
        if (Mathf.Abs(distance.x) > viewPortSize.x / 2)
        {
            targetPos.x = target.transform.position.x - (viewPortSize.x / 2 * Mathf.Sign(distance.x));
        }
        if (Mathf.Abs(distance.y) > viewPortSize.y / 2)
        {
            targetPos.y = target.transform.position.y - (viewPortSize.y / 2 * Mathf.Sign(distance.y));
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos - new Vector3(0, 0, 10), ref curVel, followDuration, maxFollowSpd);
    }

    private void OnDrawGizmos()
    {
        Color c = Color.red;
        c.a = .3f;
        Gizmos.color = c;
        Gizmos.DrawCube(transform.position, viewPortSize);
    }
}
