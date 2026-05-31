using UnityEngine;

public class BigasManager : EnemyManager
{
    public bool IsJumping  = false;
    public bool IsStomping  = false;
    public bool IsWalking  = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDead == EDeathState.Dead) Destroy(gameObject);
        if (!deactivateGrounded) { GroundDetector(); }
        if (IsDead == EDeathState.Dying || data == null) return;
        if (Lives <= 0) Die();
        FlipDirection(isFacingRight);
        raycastHit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, data.wallRayCastSize, data.groundAndPlayerMask);
        if (raycastHit.collider != null && raycastHit.collider.tag != "HitBox")
        {
            isFacingRight = !isFacingRight;
        }

        if (!IsStunned && !IsStomping) 
        {
            if (IsWalking) rb.linearVelocityX = data.MaxWalkSpeed * transform.localScale.x;
        } 
    }

    void PreJump()
    {
        IsWalking = false;
    }

    void PerformJump() 
    {
        IsWalking = true;
        rb.AddForce(Vector2.up.normalized * data.jumpForce, ForceMode2D.Impulse);
        IsJumping = false;
    }
    void EndStomp() 
    { 
        IsStomping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            Lives--;
        }
    }
}
