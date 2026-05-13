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
    private float moveSpeed;
    private float coyoteTimeCounter;
    private int _lives;
    public bool CanPlay { get; private set; }
    public bool IsStunned { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsGrounded { get; private set; }
    public Vector2 actMove { get; private set; }
    public EDeathState IsDead { get; private set; }
    public bool jumped { get; private set; }
    public float jumpBufferCounter { get; private set; }
    
    //Setters
    public void SetIsDead(EDeathState playerState) => IsDead = playerState;
    public void SetJumped(bool jumpedPar) => jumped = jumpedPar;
    public void SetBufferCounter(float counter) => jumpBufferCounter = counter;

    //Components
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundChecker;
    private AccumulationManager _accumulationManager;
    private Transform spawnPoint;

    private void Awake()
    {
        inputActionMove = InputSystem.actions.FindAction("Move");
        inputActionJump = InputSystem.actions.FindAction("Jump");
        inputActionRun = InputSystem.actions.FindAction("Sprint");
        inputActionInteract = InputSystem.actions.FindAction("Interact");
    }

    void Start()
    {
        _lives = Data.maxLives;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        rb = GetComponent<Rigidbody2D>();
        _accumulationManager = GetComponent<AccumulationManager>();
        ResetPlayer();
    }
    public void ResetPlayer()
    {
        transform.position = spawnPoint.position;
        IsDead = EDeathState.Alive;
        CanPlay = true;
        rb.simulated = true;
        GameManager.Instance.VCam.Follow = transform;
    }


    private void Update()
    {
        CheckCoyoteAndBufferTime();
        //setting run or walk state and its speed
        actMove = inputActionMove.ReadValue<Vector2>();
        IsRunning = inputActionRun.IsPressed() && IsGrounded;
        moveSpeed = IsRunning ? Data.MaxRunSpeed : Data.MaxWalkSpeed;
    }

    public void CheckCoyoteAndBufferTime() 
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
            coyoteTimeCounter = 0f;
        }

        if (inputActionJump.WasReleasedThisFrame()) coyoteTimeCounter = 0;
        #endregion
    }

    private void FixedUpdate()
    {
        if (IsDead == EDeathState.Dead) Destroy(gameObject);
        if (Data == null || !CanPlay) return;
        if (IsDead == EDeathState.Dying) 
        {
            Debug.Log("Dying in fixedupdate");
            Die();
        }
        GroundDetector();
        Move();
        if (jumped && !_accumulationManager.IsAccumulating)
        {
            Jump(1);
        }
        if (IsGrounded && rb.linearVelocity.y <= 0.1f)
        {
            rb.gravityScale = 1;
            if (!jumped) coyoteTimeCounter = Data.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
            if (rb.linearVelocity.y < 0 && coyoteTimeCounter < 0f) MultiplyGravity();
        }
    }

    private IEnumerator DyingRoutine()
    {
        yield return new WaitForSeconds(1);
        rb.simulated = false;
        yield return new WaitForSeconds(2);
        Debug.Log("Fully dead");

        IsDead = EDeathState.Dead;
        ResetPlayer();
    }
    public void Die()
    {
        IsDead = EDeathState.Dying;
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

    public void Jump(float jumpMult) 
    {
        rb.AddForce(Vector2.up.normalized * Data.jumpForce * jumpMult, ForceMode2D.Impulse);
        if (jumped) jumped = false;
        coyoteTimeCounter = 0;
    } 

    private void GroundDetector() => IsGrounded = Physics2D.OverlapCircle(groundChecker.position, Data.detectionRadius, Data.groundMask);

    private void MultiplyGravity()
    {
        rb.gravityScale = Data.gravityMult;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -Data.terminalVel));
    }

    private void Stun(Vector3 enemyPos) 
    {
        rb.AddForce((Vector3.up.normalized + (enemyPos - transform.position).normalized) * Data.selfStunKnockBack, ForceMode2D.Impulse);
        IsStunned = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") 
        {
            Stun(collision.gameObject.transform.position);
        }
    }
}
