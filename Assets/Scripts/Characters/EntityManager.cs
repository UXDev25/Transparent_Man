using System.Collections;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public Data data;
    public bool IsStunned { get; protected set; }
    public bool IsGrounded { get; protected set; }
    public EDeathState IsDead { get; protected set; }
    public int Lives { get; protected set; }
    //Setters
    public void SetIsDead(EDeathState playerState) => IsDead = playerState;

    protected bool deactivateGrounded = false;
    protected Animator animator;

    //Components
    public Rigidbody2D rb;
    [SerializeField] protected Transform groundChecker;


    void Start()
    {
        Lives = data.maxLives;
        animator = GetComponent<Animator>();
    }

    protected virtual void GroundDetector()
    {
        IsGrounded = Physics2D.OverlapCircle(groundChecker.position, data.detectionRadius, data.groundMask);
        if (IsGrounded && IsStunned)
        {
            Debug.Log("grounded && stunned");
            IsStunned = false;
        }
    }

    public virtual void Die()
    {
        IsStunned = true;
        gameObject.layer = LayerMask.NameToLayer(data.DeathMaskHash);
        IsDead = EDeathState.Dying;
        StartCoroutine(DyingRoutine());
    }

    protected virtual IEnumerator DyingRoutine()
    {
        yield return new WaitForSeconds(3);
        IsDead = EDeathState.Dead;
    }

    protected virtual void Stun(Vector3 enemyPos)
    {

        if (IsStunned) return;
        Vector2 direction = enemyPos - transform.position;
        float pseudoDirection = -Mathf.Sign(direction.x);
        Debug.Log(pseudoDirection);
        rb.AddForce(new Vector2(data.selfStunKnockBackX * pseudoDirection, data.selfStunKnockBackY), ForceMode2D.Impulse);
        Lives--;
        if (Lives <= 0)
        {
            deactivateGrounded = true;
            Die();
            return;
        }
        StartCoroutine(UnGroundedRoutine());
        IsStunned = true;
    }

    protected IEnumerator UnGroundedRoutine()
    {
        deactivateGrounded = true;
        yield return new WaitForSeconds(data.ungroundTime);
        deactivateGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, data.detectionRadius);

    }
}
