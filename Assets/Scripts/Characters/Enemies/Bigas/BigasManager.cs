using UnityEngine;

public class BigasManager : EnemyManager
{
    public bool IsJumping  = false;
    public bool IsStomping  = false;
    public bool IsWalking  = false;
    public EBigasState State;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDead == EDeathState.Dead) Destroy(gameObject);
        if (!deactivateGrounded) { GroundDetector(); }
        if (IsDead == EDeathState.Dying || data == null) return;
        if (Lives <= 0) Die();
        FlipDirection(isFacingRight);
        raycastHit = Physics2D.Raycast(transform.position, Vector2.right * -transform.localScale.x, data.wallRayCastSize, data.groundAndPlayerMask);
        if (raycastHit.collider != null && raycastHit.collider.tag != "HitBox")
        {
            isFacingRight = !isFacingRight;
        }

        if (!IsStunned && !IsStomping) 
        {
            if (State == EBigasState.Walk || !IsGrounded) rb.linearVelocityX = data.MaxWalkSpeed * -transform.localScale.x;
        }
    }
    private void OnEnable()
    {
        GameManager.OnGameReset += ResetLives;
    }

    private void OnDisable()
    {
        GameManager.OnGameReset -= ResetLives;
    }

    private void ResetLives() => Lives = data.maxLives;

    void PreJump()
    {
        State = EBigasState.Jump;
    }

    void PerformJump() 
    {
        rb.AddForce(Vector2.up.normalized * data.jumpForce, ForceMode2D.Impulse);
    }
    void ChangeDirectionRandom() 
    {
        isFacingRight = UnityEngine.Random.Range(0, 2) == 0;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            ChangeHitColor();
            Lives--;
            if (Lives == data.maxLives / 2 || Lives == data.maxLives / 4 || Lives <= 0) Stun(collision.gameObject.transform.position);
        }
    }
}
