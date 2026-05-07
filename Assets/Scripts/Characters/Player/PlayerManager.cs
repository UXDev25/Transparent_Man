using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Data Data;

    //Inputs
    private InputAction inputActionMove;
    private InputAction inputActionJump;
    private InputAction inputActionRun;
    private InputAction inputActionInteract;

    //Player
    public bool CanPlay { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsGrounded { get; private set; }
    public Vector2 actMove { get; private set; }

    private EDeathState _isDead;
    public void SetIsDead(EDeathState playerState) => _isDead = playerState;

    private bool jumped;
    private float moveSpeed;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private Transform spawnPoint;

    //Components
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundChecker;

    private void Awake()
    {
        inputActionMove = InputSystem.actions.FindAction("Move");
        inputActionJump = InputSystem.actions.FindAction("Jump");
        inputActionRun = InputSystem.actions.FindAction("Sprint");
        inputActionInteract = InputSystem.actions.FindAction("Interact");
    }

    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        rb = GetComponent<Rigidbody2D>();
        ResetPlayer();
    }
    public void ResetPlayer()
    {
        transform.position = spawnPoint.position;
        _isDead = EDeathState.Alive;
        CanPlay = true;
        rb.simulated = true;
        GameManager.Instance.VCam.Follow = transform;
    }


    private void Update()
    {
        #region coyoteTime
        if (inputActionJump.WasPressedThisFrame())
        {
            jumpBufferCounter = Data.jumpBufferTime;
        }
        else jumpBufferCounter -= Time.deltaTime;

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            jumped = true;
            jumpBufferCounter = 0f;
        }

        if (inputActionJump.WasReleasedThisFrame()) coyoteTimeCounter = 0;
        #endregion
        //setting run or walk state and its speed
        actMove = inputActionMove.ReadValue<Vector2>();
        IsRunning = inputActionRun.IsPressed() && IsGrounded;
        moveSpeed = IsRunning ? Data.MaxRunSpeed : Data.MaxWalkSpeed;
    }

    private void FixedUpdate()
    {
        if (_isDead == EDeathState.Dead) Destroy(gameObject);
        if (Data == null || !CanPlay) return;
        if (_isDead == EDeathState.Dying) 
        {
            Debug.Log("Dying in fixedupdate");
            Die();
        }
        GroundDetector();
        Move();
        if (jumped)
        {
            Jump();
            jumped = false;
        }
        if (IsGrounded)
        {
            rb.gravityScale = 1;
            coyoteTimeCounter = Data.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
            if (rb.linearVelocity.y < 0) MultiplyGravity();
        }
    }

    private IEnumerator DyingRoutine()
    {
        yield return new WaitForSeconds(1);
        rb.simulated = false;
        yield return new WaitForSeconds(2);
        Debug.Log("Fully dead");

        _isDead = EDeathState.Dead;
        ResetPlayer();
    }
    public void Die()
    {
        _isDead = EDeathState.Dying;
        CanPlay = false;
        Debug.Log("Dying start");
        StartCoroutine(DyingRoutine());
    }

    private void Move()
    {
        float targetSpeed = actMove.x * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.acceleration : Data.deceleration;
        float movement = speedDif * accelRate;
        rb.AddForce(movement * Vector2.right.normalized);
    }

    private void Jump() => rb.AddForce(Vector2.up.normalized * Data.jumpForce, ForceMode2D.Impulse);

    private void GroundDetector() => IsGrounded = Physics2D.OverlapCircle(groundChecker.position, Data.detectionRadius, Data.groundMask);

    private void MultiplyGravity()
    {
        rb.gravityScale = Data.gravityMult;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -Data.terminalVel));
    }
}
