using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: Player can get into the platform tile. -probably cuz of falling speed-
// TODO: PLayer has an animation bug, due to _isRunning bool.(i believe)
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private Animator _anim;
    private Vector2 _startPos;
    private SceneController _sceneController;

    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float jumpTimerSet;
    [SerializeField] private float coyoteTime;
    [SerializeField] private ParticleSystem movementParticle;
    [SerializeField] private ParticleSystem deathParticle;

    private bool _isFacingRight = true;
    private bool _isRunning;
    private bool _isWallSliding;
    private bool _canWallJump;
    private bool _isWallJumping;
    private bool _isAttemptingToJump;

    private float _movementInputDirection;
    private float airDrapMultiplier = 0.95f;
    private float _variableJumpHeight = 0.5f;
    private float _jumpTimer;
    private float _coyoteTimeCounter;

    private int _facingDirection = 1;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _sceneController = FindObjectOfType<SceneController>();

        _startPos = transform.position;
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfWallSliding();
        CheckJump();
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

        if (IsGrounded())
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (_coyoteTimeCounter > 0f && !_isWallSliding)
            {
                Jump();
            }
            else if(_canWallJump && !_isWallJumping && !IsGrounded())
            {
                WallJump();
            }
            else
            {
                _jumpTimer = jumpTimerSet;
                _isAttemptingToJump = true;
            }
        }
        
        if (Input.GetButtonUp("Jump"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _variableJumpHeight);
            _coyoteTimeCounter = 0f;
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

            movementParticle.Play();
        }
    }

    private void Flip()
    {
        _facingDirection *= -1;
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);

        movementParticle.Play();
    }

    private void CheckJump()
    {
        if (_jumpTimer > 0)
        {
            if (_canWallJump && !_isWallJumping && !IsGrounded())
            {
                WallJump();
            }
            else if (IsGrounded() && !_isWallSliding)
            {
                Jump();
            }
        }

        if (_isAttemptingToJump)
        {
            jumpTimerSet -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

        _jumpTimer = 0;
        _isAttemptingToJump = false;

        movementParticle.Play();
    }

    private void WallJump()
    {
        _isWallJumping = true;

        int wallDirection = IsTouchingWall() ? (int)Mathf.Sign(_rb.velocity.x) : (_isFacingRight ? -1 : 1);

        _rb.velocity = new Vector2(wallDirection * wallJumpForce, jumpForce);

        Invoke("ResetWallJump", 0.2f);

        _jumpTimer = 0;
        _isAttemptingToJump = false;
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

            movementParticle.Play();
        }
        else
        {
            _isWallSliding = false;
            _canWallJump = false;
        }
    }

    private void Die()
    {
        _sceneController.SetDeathScreen();
        StartCoroutine(Respawn(1));
    }

    private IEnumerator Respawn(float duration)
    {
        _rb.velocity = new Vector2(0, 0);
        _rb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        transform.position = _startPos;
        transform.localScale = new Vector3(1, 1, 1);
        _rb.simulated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            deathParticle.Play();
            Die();
        }
    }
}
