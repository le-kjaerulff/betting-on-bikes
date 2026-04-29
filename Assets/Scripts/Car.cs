using System;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class Car : MonoBehaviour
{
    public Transform endPoint;
    public float speed = 0;

    void Start()
    {
        if (endPoint == null) return;
        Vector3 moveDirection  = (endPoint.position - transform.position).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if(endPoint == null) return;
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, (speed * Time.deltaTime));

        if (Vector3.Distance(transform.position, endPoint.position) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}

