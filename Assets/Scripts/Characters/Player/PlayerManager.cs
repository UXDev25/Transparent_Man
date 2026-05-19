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

    //Player
    private float coyoteTimeCounter;
    private int _lives;
    private bool _deactivateGrounded = false;
    public bool CanPlay { get; private set; }
    public bool IsStunned { get; private set; }
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
    public Rigidbody2D rb { get; private set; }
    [SerializeField] private Transform groundChecker;
    private Transform spawnPoint;

    private void Awake()
    {
        inputActionMove = InputSystem.actions.FindAction("Move");
        inputActionJump = InputSystem.actions.FindAction("Jump");
        inputActionRun = InputSystem.actions.FindAction("Sprint");
    }

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(Data.LifeMaskHash);
        _lives = Data.maxLives;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        rb = GetComponent<Rigidbody2D>();
        ResetPlayer();
    }
    public void ResetPlayer()
    {
        transform.position = spawnPoint.position;
        gameObject.layer = LayerMask.NameToLayer(Data.LifeMaskHash);
        _lives = Data.maxLives;
        IsDead = EDeathState.Alive;
        CanPlay = true;
        IsStunned = false;
        _deactivateGrounded = false;
        rb.simulated = true;
        GameManager.Instance.VCam.Follow = transform;
    }


    private void Update()
    {
        CheckCoyoteAndBufferTime();
        //setting run or walk state and its speed
        actMove = inputActionMove.ReadValue<Vector2>();
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
        if (!_deactivateGrounded) { GroundDetector(); }
        if (IsDead == EDeathState.Dying || Data == null || !CanPlay) return;
        if (_lives <= 0) Die();
        Move();
        if (jumped)
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
        yield return new WaitForSeconds(3);
        Debug.Log("Fully dead");
        IsDead = EDeathState.Dead;
        ResetPlayer();
    }
    public void Die()
    {
        IsStunned = true;
        CanPlay = false;
        GameManager.Instance.VCam.Follow = null;
        gameObject.layer = LayerMask.NameToLayer(Data.DeathMaskHash);
        Debug.Log(gameObject.layer.ToString());
        IsDead = EDeathState.Dying;
        
        Debug.Log("Dying start");
        StartCoroutine(DyingRoutine());
    }

    private void Move()
    {
        float targetSpeed = actMove.x * Data.MaxWalkSpeed;
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

    private void GroundDetector() 
    {
        IsGrounded = Physics2D.OverlapCircle(groundChecker.position, Data.detectionRadius, Data.groundMask);
        if (IsGrounded && IsStunned) 
        {
            Debug.Log("grounded && stunned");
            IsStunned = false;
            CanPlay = true;
        } 
    } 

    private void MultiplyGravity()
    {
        rb.gravityScale = Data.gravityMult;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -Data.terminalVel));
    }

    private void Stun(Vector3 enemyPos) 
    {
        
        if (IsStunned) return;
        Vector2 direction = enemyPos - transform.position;
        float pseudoDirection = -Mathf.Sign(direction.x);
        rb.AddForce(new Vector2(Data.selfStunKnockBackX * pseudoDirection, Data.selfStunKnockBackY), ForceMode2D.Impulse);
        _lives--;
        if (_lives <= 0) 
        {
            _deactivateGrounded = true;
            Die();
            return;
        } 
        StartCoroutine(UnGroundedRoutine());
        IsStunned = true;
        CanPlay = false;
    }

    private IEnumerator UnGroundedRoutine()
    {
        _deactivateGrounded = true;
        yield return new WaitForSeconds(1f);
        _deactivateGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !IsStunned) 
        {
            Stun(collision.gameObject.transform.position);
        }
    }
}
