using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Data data;

    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundChecker.position, data.detectionRadius, data.groundMask); 
    }
}
