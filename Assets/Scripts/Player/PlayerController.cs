using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;

    private Animator _anim;
    private bool _isRunning;

    private float _movementInputDirection;
    private bool _isFacingRight = true;
    [SerializeField] private LayerMask platformLayer;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0)
        {
            Flip();
        }
        else if (!_isFacingRight && _movementInputDirection > 0)
        {
            Flip();
        }

        if (_rb.velocity.x != 0)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        _anim.SetBool("_isRunning", _isRunning);
        _anim.SetBool("isGrounded", IsGrounded());
        _anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {
        _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }

    private bool IsGrounded()
    {
        float _extraHeight = 0.3f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            _collider.bounds.center, _collider.bounds.size, 0 ,Vector2.down, _extraHeight, platformLayer);

        return raycastHit.collider != null;
    }
}
