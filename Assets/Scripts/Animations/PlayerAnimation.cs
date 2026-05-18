using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsAccumulatingHash = Animator.StringToHash("isAccumulating");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private PlayerManager player;
    private AccumulationManager _accumulationManager;
    private Rigidbody2D playerRb;
    private Animator animator;

    private bool isMidAir;
    private bool isAccumulating;
    private bool isWalking;
    private bool isRunning;
    private bool isStunned;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        _accumulationManager = GetComponent<AccumulationManager>();
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!player.IsStunned) FlipDirection();
        isWalking = !isMidAir && !player.IsRunning && player.actMove.x != 0;
        animator.SetBool(IsWalkingHash, isWalking);
        isRunning = !isMidAir && player.IsRunning && player.actMove.x != 0;
        animator.SetBool(IsRunningHash, isRunning);
        isMidAir = !player.IsGrounded;
        animator.SetBool(IsMidAirHash, isMidAir);
        isAccumulating = _accumulationManager.IsPressingDown();
        animator.SetBool(IsAccumulatingHash, isAccumulating);
        isStunned = player.IsStunned;
        animator.SetBool(IsStunnedHash, isStunned);
        //Debug.Log($"is walking: {isWalking}, isMidAir: {isMidAir}");
    }

    private void FlipDirection() 
    {
        if (player.actMove.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.actMove.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
