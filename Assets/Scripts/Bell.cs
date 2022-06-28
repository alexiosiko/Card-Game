using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public float speed = 5;
    public Vector3 target;
    public bool animate = false;
    void Update()
    {
        if (animate == true)
            transform.parent.position = Vector3.Lerp(transform.parent.position, target, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target) < 0.1f)
            animate = false;
    }
}
