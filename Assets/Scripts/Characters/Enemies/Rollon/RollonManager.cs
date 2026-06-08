using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RollonManager : EnemyManager
{
    //Components
    [SerializeField] private Transform _edgeChecker;
    

    private void FixedUpdate()
    {
        if (IsDead == EDeathState.Dead) Destroy(gameObject);
        if (!deactivateGrounded) { GroundDetector(); }
        if (IsDead == EDeathState.Dying || data == null) return;
        if (Lives <= 0) Die();
        FlipDirection(isFacingRight);
        if (!IsStunned) rb.linearVelocityX = data.MaxWalkSpeed * transform.localScale.x;
        raycastHit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, data.wallRayCastSize, data.groundAndPlayerMask);
        if (((raycastHit.collider != null && raycastHit.collider.tag != "HitBox") || IsNearEdge()) && IsGrounded && !IsStunned)
        {
            isFacingRight = !isFacingRight;
        }
    }

    

    private bool IsNearEdge() 
    {
        Vector3 targetPos = _edgeChecker.position;
        targetPos.y -= data.detectionRadius;

        Debug.DrawLine(_edgeChecker.position, targetPos, Color.green);
        
        return !Physics2D.Linecast(_edgeChecker.position, targetPos, data.groundMask);
    }
}
