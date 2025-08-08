using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoBOSS : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem movementPatricle1;
    [SerializeField] private ParticleSystem movementPatricle2;

    private Vector3 _targetPoint;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        if (pointA != null)
        {
            _targetPoint = pointA.position;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (pointA != null && pointB != null)
        {
            movementPatricle1.Play();
            movementPatricle2.Play();
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPoint) < 0.1f)
            {
                StartCoroutine(WaitBeforeCharging());
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
    private IEnumerator WaitBeforeCharging()
    {
        yield return new WaitForSeconds(1f);
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
