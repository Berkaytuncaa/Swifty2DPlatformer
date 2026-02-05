using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;

    private Vector3 _targetPoint;


    void Start()
    {
        if (pointA != null)
        {
            _targetPoint = pointA.position;
        }
    }

    void FixedUpdate()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPoint) < 0.1f)
            {
                _targetPoint = _targetPoint == pointA.position ? pointB.position : pointA.position;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

}
