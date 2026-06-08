using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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
    public string groundMaskHash = "Ground";
    public float detectionRadius = 0.2f;

    [Header("EntityPhysics")]
    public float MaxWalkSpeed = 6;
    public float acceleration = 500f;
    public float deceleration = 340f;
    public float velPower = 6;
    public float jumpForce = 100;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    

    [Header("KnockBacks")]
    public Vector2 comboPunchKB = new Vector2(5, 3);
    public Vector2 punchKB = new Vector2(5, 3);
    public float selfStunKnockBackX = 5;
    public float selfStunKnockBackY = 3;
    public float punchForwardForce = 2;
    public float punchHitRetreat = 3.5f;

    [Header("Times & Cooldowns")]
    public float ungroundTime = 0.5f;

    [Header("UI")]
    public float lifeUiSeparator = 20;

    [Header("Life And Others")]
    public int maxLives = 3;
    public Color hitColor = Color.red;
    public float hitColorDuration = 0.1f;

    [Header("Enemies")]
    public float wallRayCastSize = 0.5f;
    public LayerMask groundAndPlayerMask;
    public string playerTag = "Player";
}
