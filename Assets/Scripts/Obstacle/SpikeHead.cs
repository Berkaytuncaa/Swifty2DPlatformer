using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;
    [SerializeField] private float waitingDuration = 0.5f;
    private float _ourSpeed;

    private Vector3 _targetPoint;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _ourSpeed = speed;
        if (pointA != null)
        {
            _targetPoint = pointA.position;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPoint) < 0.1f)
            {
                StartCoroutine(Wait());
                _targetPoint = _targetPoint == pointA.position ? pointB.position : pointA.position;
            }
        }

        if (_targetPoint == pointA.position)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_targetPoint == pointB.position)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private IEnumerator Wait()
    {
        speed = 0;
        yield return new WaitForSeconds(waitingDuration);
        speed = _ourSpeed;
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
}
