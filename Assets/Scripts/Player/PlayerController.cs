using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: Player can do vertical jumps on the same wall, we do not want that. It only should be able to jump to the opposite wall.
// TODO: Player can get into the platform tile. -probably cuz of falling speed-
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private Animator _anim;
    [SerializeField] private LayerMask platformLayer;

    private bool _isFacingRight = true;
    private bool _isRunning;
    private bool _isWallSliding;
    private bool _canWallJump;
    private bool _isWallJumping;

    private float _movementInputDirection;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpForce;
    private float airDrapMultiplier = 0.95f;
    private float _variableJumpHeight = 0.5f;

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
        CheckIfWallSliding();
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

        _isRunning = Mathf.Abs(_rb.velocity.x) > 0;
}

    private void UpdateAnimations()
    {
        _anim.SetBool("_isRunning", _isRunning);
        _anim.SetBool("isGrounded", IsGrounded());
        _anim.SetFloat("yVelocity", _rb.velocity.y);
        _anim.SetBool("isWallSliding", _isWallSliding);
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                Jump();
            }
            else if(_canWallJump && !_isWallJumping && !IsGrounded())
            {
                WallJump();
            }
        }
        
        if (Input.GetButtonUp("Jump"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _variableJumpHeight);
        }
    }

    private void ApplyMovement()
    {
        if (!IsGrounded() && _isWallSliding && _movementInputDirection == 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x * airDrapMultiplier, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);
        }

        if (_isWallSliding && _rb.velocity.y < -wallSlidingSpeed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed);
        }
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

    private void WallJump()
    {
        _isWallJumping = true;

        int wallDirection = IsTouchingWall() ? (int)Mathf.Sign(_rb.velocity.x) : (_isFacingRight ? -1 : 1);

        _rb.velocity = new Vector2(wallDirection * wallJumpForce, jumpForce);

        Invoke("ResetWallJump", 0.2f);
    }

    private void ResetWallJump()
    {
        _isWallJumping = false;
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.3f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            _collider.bounds.center, _collider.bounds.size, 0 ,Vector2.down, extraHeight, platformLayer);

        return raycastHit.collider != null;
    }

    private bool IsTouchingWall()
    {
        float extraLenght = 0.1f;
        RaycastHit2D rightRayCastHit = Physics2D.Raycast(
            _collider.bounds.center, Vector2.right, _collider.bounds.extents.x + extraLenght, platformLayer);
        RaycastHit2D leftRayCastHit = Physics2D.Raycast(
            _collider.bounds.center, Vector2.left, _collider.bounds.extents.x + extraLenght, platformLayer);

        return rightRayCastHit.collider != null || leftRayCastHit.collider != null;
    }

    private void CheckIfWallSliding()
    {
        if (IsTouchingWall() && !IsGrounded())
        {
            _isWallSliding = true;
            _canWallJump = true;
        }
        else
        {
            _isWallSliding = false;
            _canWallJump = false;
        }
    }
}
