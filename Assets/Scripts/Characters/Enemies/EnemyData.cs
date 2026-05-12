using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("GeneralPhysics")]
    public float gravityMult = 1.5f;
    public float terminalVel = 100f;

    [Header("GroundChecker")]
    public LayerMask groundMask;
    public Vector2 rayCastOffset;

    [Header("Physics")]
    public float MaxRunSpeed = 12;
    public float MaxWalkSpeed = 6;
    public float acceleration = 500f;
    public float deceleration = 340f;
    public float velPower = 6;
    public float jumpForce = 100;

}
