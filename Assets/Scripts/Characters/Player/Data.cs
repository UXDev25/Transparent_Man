using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Data")]
public class Data : ScriptableObject
{
    [Header("GeneralPhysics")]
    public float gravityMult = 1.5f;
    public float terminalVel = 100f;
    public string DeathMaskHash;
    public string LifeMaskHash;

    [Header("GroundChecker")]
    public LayerMask groundMask;
    public float detectionRadius = 0.2f;

    [Header("PlayerPhysics")]
    public float MaxRunSpeed = 12;
    public float MaxWalkSpeed = 6;
    public float acceleration = 500f;
    public float deceleration = 340f;
    public float velPower = 6;
    public float jumpForce = 100;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    public float maxJumpAccum = 5;
    public float selfStunKnockBackX = 5;
    public float selfStunKnockBackY = 3;

    [Header("Times & Cooldowns")]
    public float maxTimeAccum = 1;
    public float jumpMultAccum = 0.25f;

    [Header("Life And Others")]
    public int maxLives = 3;
}
