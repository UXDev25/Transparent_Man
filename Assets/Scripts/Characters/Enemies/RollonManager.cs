using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RollonManager : EnemyManager
{
    //Components
    [SerializeField] private Transform _edgeChecker;
    private bool _isFacingRight = true;
    private RaycastHit2D _raycastHit;

    private void FixedUpdate()
    {
        if (IsDead == EDeathState.Dead) Destroy(gameObject);
        if (!deactivateGrounded) { GroundDetector(); }
        if (IsDead == EDeathState.Dying || data == null) return;
        if (Lives <= 0) Die();
        FlipDirection(_isFacingRight);
        if (!IsStunned) rb.linearVelocityX = data.MaxWalkSpeed * transform.localScale.x;
        _raycastHit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, data.wallRayCastSize, data.groundAndPlayerMask);
        if (((_raycastHit.collider != null && _raycastHit.collider.tag != "HitBox") || IsNearEdge()) && IsGrounded && !IsStunned)
        {
            _isFacingRight = !_isFacingRight;
        }
    }


    private bool IsNearEdge() 
    {
        Vector3 targetPos = _edgeChecker.position;
        targetPos.y -= data.detectionRadius;

        Debug.DrawLine(_edgeChecker.position, targetPos, Color.green);
        
        return !Physics2D.Linecast(_edgeChecker.position, targetPos, data.groundMask);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            Stun(collision.gameObject.transform.GetChild(0).position, collision.gameObject.GetComponent<HitboxInfo>().KnockBack);
        }
    }
}
