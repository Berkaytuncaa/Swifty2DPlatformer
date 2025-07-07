using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckObstacle : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public float prepareDelay = 0.2f;
    public float timeBetweenJumps = 1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private bool _isGrounded => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        JumpMotion();
    }

    private void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", _isGrounded);

        if (_isGrounded)
        {
            StartCoroutine(WaitBetweenJumps());
        }
    }

    private IEnumerator WaitBetweenJumps()
    {
        yield return new WaitForSeconds(timeBetweenJumps);
        JumpMotion();
    }

    private void JumpMotion()
    {
        StartCoroutine(PrepareForJump());
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private IEnumerator PrepareForJump()
    {
        yield return new WaitForSeconds(prepareDelay);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
