using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    #region References
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private Animator _anim;
    private Vector2 _startPos;
    private SceneController _sceneController;
    private AudioManager audioManager;
    private CinemachineImpulseSource _impulseSource;
    #endregion

    #region Serialized Fields
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private ParticleSystem movementParticle;
    [SerializeField] private ParticleSystem deathParticle;
    #endregion

    #region State Flags
    private bool _canMove = true;
    private bool _isFacingRight = true;
    private bool _isRunning;
    private bool _isWallSliding;
    private bool _canWallJump;
    private bool _isWallJumping;
    private bool _isAttemptingToJump;
    #endregion

    #region Movement Variables
    private float _movementInputDirection;
    private float _variableJumpHeight = 0.5f;
    private float _coyoteTime = 0.2f;
    private float _jumpTimer;
    private float _jumpTimerSet = 0.15f;
    private float _coyoteTimeCounter;
    private int _facingDirection = 1;
    #endregion

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _sceneController = FindObjectOfType<SceneController>();

        _startPos = transform.position;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfWallSliding();
        //CheckJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0 && !_isWallSliding)
        {
            Flip();
        }
        else if (!_isFacingRight && _movementInputDirection > 0 && !_isWallSliding)
        {
            Flip();
        }

        _isRunning = Mathf.Abs(_movementInputDirection) > 0;
    }

    private void UpdateAnimations()
    {
        if (_isRunning && IsGrounded())
        {
            audioManager.PlayMovementSFX();
        }
        else
        {
            audioManager.StopMovementSFX();
        }

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
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_coyoteTimeCounter > 0f && !_isWallSliding)
            {
                Jump();
                //Debug.Log("I have just jumped");
            }
            else if(_canWallJump && !_isWallJumping && !IsGrounded())
            {
                WallJump();
                //Debug.Log("I have just wall-jumped");
            }
            else
            {
                _jumpTimer = _jumpTimerSet;
                _isAttemptingToJump = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _variableJumpHeight);
            _coyoteTimeCounter = 0f;
        }
    }

    private void ApplyMovement()
    {
        if (_isWallSliding && _rb.velocity.y < -wallSlidingSpeed && Input.GetAxisRaw("Vertical") == 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed);

            movementParticle.Play();
        }
        else if(_canMove)
        {
            _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);
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
            if (_canWallJump && !_isWallJumping && !IsGrounded() && _isWallSliding)
            {
                WallJump();
            }
            else if (IsGrounded() && !_isWallSliding && !_isWallJumping)
            {
                Jump();
            }
        }

        if (_isAttemptingToJump)
        {
            _jumpTimerSet -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

        _jumpTimer = 0;
        _isAttemptingToJump = false;

        audioManager.PlaySFX(audioManager.jump);
        movementParticle.Play();
    }

    private void WallJump()
    {
        _isWallJumping = true;

        Vector2 direction = new Vector2(wallJumpDirection.x * -_facingDirection, wallJumpDirection.y);
        _rb.AddForce(direction, ForceMode2D.Impulse);

        Invoke("ResetWallJump", 0.2f);

        audioManager.PlaySFX(audioManager.jump);

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
            _collider.bounds.center, new Vector3(_collider.bounds.size.x / 1.1f, _collider.bounds.size.y, _collider.bounds.size.z), 0, Vector2.down, extraHeight, platformLayer);

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
            _canMove = false;

            movementParticle.Play();
        }
        else
        {
            _isWallSliding = false;
            _canWallJump = false;
            _canMove = true;
        }
    }

    public void Die()
    {
        audioManager.PlaySFX(audioManager.death);
        CameraShakeManager.instance.CameraShake(_impulseSource);
        _sceneController.SetDeathScreen();

        StartCoroutine(Respawn(1.3f));
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

    private void OnDrawGizmos()
    {
        if (_collider == null) return;

        float extraHeight = 0.3f;
        Gizmos.color = Color.red;

        Vector2 boxCastPos = (Vector2)_collider.bounds.center + Vector2.down * extraHeight / 2;

        Gizmos.DrawWireCube(boxCastPos, new Vector2(_collider.bounds.size.x / 1.1f, _collider.bounds.size.y + extraHeight));
    }
}
