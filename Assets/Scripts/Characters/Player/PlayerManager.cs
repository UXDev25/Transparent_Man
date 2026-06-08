using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : EntityManager
{
    

    //Inputs
    private InputAction inputActionMove;
    private InputAction inputActionJump;

    //Player
    private float coyoteTimeCounter;
    public bool CanPlay { get; private set; }
    public Vector2 actMove { get; private set; }
    public bool jumped { get; private set; }
    public float jumpBufferCounter { get; private set; }
    [SerializeField] private GameObject _lifePlace;
    [SerializeField] private GameObject _lifeIconPrefab;

    private int _lives;
    public override int Lives { get => _lives;
        protected set
        {
            if (_lives < value && _lives != 0) 
            {
                Destroy(_lifePlace.transform.GetChild(0).gameObject);
            }
        } 
    }

    //Setters
    public void SetJumped(bool jumpedPar) => jumped = jumpedPar;
    public void SetCanPlay(bool canPlay) => CanPlay = canPlay;
    public void SetBufferCounter(float counter) => jumpBufferCounter = counter;

    //Components
    private Transform spawnPoint;

    private void Awake()
    {
        inputActionMove = InputSystem.actions.FindAction("Move");
        inputActionJump = InputSystem.actions.FindAction("Jump");
    }

    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer(data.LifeMaskHash);
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        ResetPlayer();
    }
    public void ResetPlayer()
    {
        int i = 0;
        while (i <= data.maxLives) 
        {
            GameObject lifePref = Instantiate(_lifeIconPrefab, _lifePlace.transform);
            RectTransform rectPrefTrans = lifePref.GetComponent<RectTransform>();
            rectPrefTrans.position = new Vector3(rectPrefTrans.position.x + data.lifeUiSeparator, rectPrefTrans.position.y, rectPrefTrans.position.z);
            i++;
        }
        _audioManager.StartMusic();
        transform.position = spawnPoint.position;
        gameObject.layer = LayerMask.NameToLayer(data.LifeMaskHash);
        Lives = data.maxLives;
        IsDead = EDeathState.Alive;
        CanPlay = true;
        IsStunned = false;
        deactivateGrounded = false;
        rb.simulated = true;
        GameManager.Instance.VCam.Follow = transform;
        GameManager.Instance.ResetedGame = true;
    }


    private void Update()
    {
        if (GameManager.Instance.PauseCharacter) return;
        CheckCoyoteAndBufferTime();
        //setting run or walk state and its speed
        actMove = inputActionMove.ReadValue<Vector2>();
    }

    public void CheckCoyoteAndBufferTime() 
    {
        #region coyoteTime
        if (inputActionJump.WasPressedThisFrame())
        {
            jumpBufferCounter = data.jumpBufferTime;
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
        if (GameManager.Instance.PauseCharacter) 
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        if (IsDead == EDeathState.Dead) Destroy(gameObject);
        if (!deactivateGrounded) { GroundDetector(); }
        if (IsDead == EDeathState.Dying || data == null || !CanPlay) return;
        if (Lives <= 0) Die();
        Move();
        if (jumped)
        {
            Jump(1);
        }
        if (IsGrounded && rb.linearVelocity.y <= 0.1f)
        {
            rb.gravityScale = 1;
            if (!jumped) coyoteTimeCounter = data.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
            if (rb.linearVelocity.y < 0 && coyoteTimeCounter < 0f) MultiplyGravity();
        }
    }

    protected override IEnumerator DyingRoutine()
    {
        _audioManager.FadeOutMusic();
        yield return new WaitForSeconds(3);
        Debug.Log("Fully dead");
        IsDead = EDeathState.Dead;
        ResetPlayer();
    }
    public override void Die()
    {

        IsStunned = true;
        CanPlay = false;
        GameManager.Instance.VCam.Follow = null;
        gameObject.layer = LayerMask.NameToLayer(data.DeathMaskHash);
        Debug.Log(gameObject.layer.ToString());
        IsDead = EDeathState.Dying;
        
        Debug.Log("Dying start");
        StartCoroutine(DyingRoutine());
    }

    private void Move()
    {
        float targetSpeed = actMove.x * data.MaxWalkSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration : data.deceleration;
        float movement = speedDif * accelRate;
        rb.AddForce(movement * Vector2.right.normalized);
    }

    public void Jump(float jumpMult) 
    {
        rb.AddForce(Vector2.up.normalized * data.jumpForce * jumpMult, ForceMode2D.Impulse);
        if (jumped) jumped = false;
        coyoteTimeCounter = 0;
    }

    protected override void GroundDetector() 
    {
        IsGrounded = Physics2D.OverlapCircle(groundChecker.position, data.detectionRadius, data.groundMask);
        if (IsGrounded && IsStunned) 
        {
            Debug.Log("grounded && stunned");
            IsStunned = false;
            CanPlay = true;
        } 
    } 

    private void MultiplyGravity()
    {
        rb.gravityScale = data.gravityMult;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -data.terminalVel));
    }

    protected override void Stun(Vector3 enemyPos) 
    {
        base.Stun(enemyPos);
        CanPlay = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();   
        if (enemy != null && enemy.tag == "Enemy" && !IsStunned && !enemy.IsStunned)
        {
            _audioManager.PlayHitSFX();
            ChangeHitColor();
            Stun(collision.gameObject.transform.position);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("BigasArea"))
        {
            _audioManager.StartBigasMusic();
        }
    }

}
