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

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    private bool _isJumpingSequenceActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(JumpSequence());
    }

    private void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", IsGrounded);
    }

    private IEnumerator JumpSequence()
    {
        if (_isJumpingSequenceActive) yield break;
        _isJumpingSequenceActive = true;

        while (true)
        {
            yield return new WaitUntil(() => IsGrounded);

            yield return new WaitForSeconds(timeBetweenJumps);

            yield return new WaitForSeconds(prepareDelay); // Wait for the preparation

            // 4. Perform the actual jump (apply force immediately after preparation)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // animator.SetTrigger("Jump"); // Example: Trigger jump animation

            // The obstacle will naturally fall due to gravity after this
            // The loop will then restart and wait until it's grounded again
        }
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
