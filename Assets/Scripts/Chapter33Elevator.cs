using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter33Elevator : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;

    private Vector3 _targetPoint;
    private bool _playerOnBoard = false;
    private bool _lockedAtB = false;

    // call it from your player death event
    public static Chapter33Elevator Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (pointA != null)
            _targetPoint = pointA.position;
    }

    void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

        // State: locked at point B, do nothing
        if (_lockedAtB) return;

        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, speed * Time.deltaTime);

        bool reachedTarget = Vector3.Distance(transform.position, _targetPoint) < 0.1f;

        if (reachedTarget)
        {
            // Reached point B with player → lock it
            if (_targetPoint == pointB.position && _playerOnBoard)
            {
                _lockedAtB = true;
                return;
            }

            // Reached point B without player (shouldn't normally happen) → go back
            if (_targetPoint == pointB.position && !_playerOnBoard)
            {
                _targetPoint = pointA.position;
                return;
            }

            // Reached point A → just wait, don't auto-move
            // Elevator will only move toward B when player boards
        }
    }

    // Call this from player death method
    public void OnPlayerDied()
    {
        _playerOnBoard = false;
        _lockedAtB = false;
        _targetPoint = pointA.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        collision.transform.SetParent(transform);
        _playerOnBoard = true;

        // Only start moving toward B if not locked and not already heading there
        if (!_lockedAtB)
            _targetPoint = pointB.position;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        collision.transform.SetParent(null);
        _playerOnBoard = false;

        // If player dropped off before reaching B, return to A
        if (!_lockedAtB && _targetPoint == pointB.position)
            _targetPoint = pointA.position;
    }

    void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.position, pointB.position);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pointA.position, 0.1f);
        Gizmos.DrawSphere(pointB.position, 0.1f);
    }
}
