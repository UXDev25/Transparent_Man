using UnityEngine;

public class RollonManager : MonoBehaviour
{
    public EnemyData enemyData;
    private Rigidbody2D _rb;
    private GroundSensor _groundSensor;
    [SerializeField] private float _raycastLength;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponent<GroundSensor>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocityX = enemyData.MaxWalkSpeed;
        RaycastHit2D hit = Physics2D.Raycast(enemyData.rayCastOffset, Vector2.down, _raycastLength, enemyData.groundMask);
        if (hit == null && _groundSensor.IsGrounded) 
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        }
    }
}
