using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private Transform _groundChecker;
    private Rigidbody2D _rb;
    private RaycastHit2D _raycastHit;
    public bool IsOnGround { get; private set; }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundChecker.position, _data.detectionRadius);
        
    }
}
