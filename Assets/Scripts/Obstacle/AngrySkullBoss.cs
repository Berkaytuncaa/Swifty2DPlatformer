using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngrySkullBoss : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;

    private float _ourSpeed;
    private float waitingDuration = 1.3f;
    private Vector3 _targetPoint;
    private SpriteRenderer _spriteRenderer;

    public static AngrySkullBoss Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (pointA != null)
        {
            _ourSpeed = speed;
            _targetPoint = pointA.position;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPoint) < 0.1f)
            {
                transform.position = _targetPoint;
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

    public void OnPlayerDied()
    {
        StartCoroutine(Wait());
        // transform.position = pointB.position;
    }

    private IEnumerator Wait()
    {
        speed = 0;
        yield return new WaitForSeconds(waitingDuration);
        transform.position = pointB.position;
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
