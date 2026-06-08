using System;
using System.Collections;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public Data data;
    public bool IsStunned { get; protected set; }
    public bool IsGrounded { get; protected set; }
    public EDeathState IsDead { get; protected set; }
    public virtual int Lives { get; protected set; }
    //Setters
    public void SetIsDead(EDeathState playerState) => IsDead = playerState;

    protected bool deactivateGrounded = false;
    protected Animator animator;

    //Components
    public Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Transform groundChecker;
    protected AudioManager _audioManager;

    protected virtual void Start()
    {
        Lives = data.maxLives;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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

    protected virtual void Stun(Vector3 enemyPos, Vector2 knockBack)
    {
        Vector2 direction = enemyPos - transform.position;
        float pseudoDirection = -Mathf.Sign(direction.x);
        rb.AddForce(new Vector2(knockBack.x * pseudoDirection, knockBack.y), ForceMode2D.Impulse);
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

    //Stun method for player
    protected virtual void Stun(Vector3 enemyPos)
    {

        if (IsStunned) return;
        Vector2 direction = enemyPos - transform.position;
        float pseudoDirection = -Mathf.Sign(direction.x);
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

    protected void ChangeHitColor()
    {
        StartCoroutine(ReturnToColor());
    }

    private IEnumerator ReturnToColor() 
    {
        spriteRenderer.color = data.hitColor;
        yield return new WaitForSeconds(data.hitColorDuration);
        spriteRenderer.color = Color.white;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitBox"))
        {
            _audioManager.PlayHitSFX();
            ChangeHitColor();
            Stun(collision.gameObject.transform.GetChild(0).position, collision.gameObject.GetComponent<HitboxInfo>().KnockBack);
        }
    }
}
