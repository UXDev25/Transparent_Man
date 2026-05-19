using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private static readonly int IsPunchingHash = Animator.StringToHash("isPunching");
    private static readonly int punchCounterHash = Animator.StringToHash("punchCounter");
    private PlayerManager player;
    private Rigidbody2D playerRb;
    private Animator animator;
    private HitManager hitManager;

    private bool isMidAir;
    private bool isWalking;
    private bool isStunned;
    private bool isPunching;
    private int punchCounter;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        hitManager = GetComponent<HitManager>();
    }

    void Update()
    {
        if (!player.IsStunned) FlipDirection();
        isWalking = !isMidAir && player.actMove.x != 0;
        animator.SetBool(IsWalkingHash, isWalking);
        isMidAir = !player.IsGrounded;
        animator.SetBool(IsMidAirHash, isMidAir);
        isStunned = player.IsStunned;
        animator.SetBool(IsStunnedHash, isStunned);
        isPunching = hitManager.IsPunching;
        animator.SetBool(IsPunchingHash, isPunching);
        animator.SetInteger(punchCounterHash, hitManager.PunchCounter);
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
