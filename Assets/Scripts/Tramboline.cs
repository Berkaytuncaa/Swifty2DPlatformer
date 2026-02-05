using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tramboline : MonoBehaviour
{
    [SerializeField] private float _jumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
