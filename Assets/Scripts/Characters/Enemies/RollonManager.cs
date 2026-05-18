using UnityEngine;
[RequireComponent(typeof(EnemyManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class RollonManager : MonoBehaviour
{
    //Components
    [SerializeField] private Data _data;
    private Rigidbody2D _rb;
    private EnemyManager _enemyManager;
    private Animator _animator;
    [SerializeField] private Transform _groundChecker;
    public bool IsOnGround { get; private set; }

    private bool _isFacingRight = true;
    private RaycastHit2D _raycastHit;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyManager = GetComponent<EnemyManager>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsOnGroundDetector();
        FlipDirection(_isFacingRight);
        _rb.linearVelocityX = _data.MaxWalkSpeed * transform.localScale.x;
        Debug.Log(IsOnGround);
        _raycastHit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, _data.wallRayCastSize, _data.groundMask);
        if (_raycastHit.collider != null || !IsOnGround)
        {
            _isFacingRight = !_isFacingRight;
        }
    }

    public void FlipDirection(bool enemyCondition)
    {
        transform.localScale = enemyCondition ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }

    private void IsOnGroundDetector()
    {
        IsOnGround = Physics2D.OverlapCircle(_groundChecker.position, _data.detectionRadius, _data.groundMask);
    }
}
