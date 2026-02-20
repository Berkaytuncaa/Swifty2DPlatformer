using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float speed;
    [SerializeField] private bool moveRight;
    [SerializeField] private bool moveDown;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        if (moveRight)
        {
            _rb.velocity = Vector2.right * speed;
        }
        else if (moveDown)
        {
            _rb.velocity = Vector2.down * speed;
        }
        else
        {
            _rb.velocity = Vector2.left * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ShooterPlant"))
        {
            Destroy(gameObject);
        }
    }
}
