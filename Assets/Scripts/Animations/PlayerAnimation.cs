using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsAccumulatingHash = Animator.StringToHash("isAccumulating");
    private PlayerManager player;
    private AccumulationManager _accumulationManager;
    private Rigidbody2D playerRb;
    private Animator animator;

    private bool isMidAir;
    private bool isAccumulating;
    private bool isWalking;
    private bool isRunning;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        _accumulationManager = GetComponent<AccumulationManager>();
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FlipDirection();
        isWalking = !isMidAir && !player.IsRunning && player.actMove.x != 0;
        animator.SetBool(IsWalkingHash, isWalking);
        isRunning = !isMidAir && player.IsRunning && player.actMove.x != 0;
        animator.SetBool(IsRunningHash, isRunning);
        isMidAir = !player.IsGrounded;
        animator.SetBool(IsMidAirHash, isMidAir);
        isAccumulating = _accumulationManager.IsPressingDown();
        animator.SetBool(IsAccumulatingHash, isAccumulating);
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
