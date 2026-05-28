using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private PlayerManager player;
    private HitManager hitManager;
    private Animator animator;

    private bool isMidAir;
    private bool isWalking;
    private bool isStunned;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        hitManager = GetComponent<HitManager>();
    }

    void Update()
    {
        if (!player.IsStunned && player.CanPlay) FlipDirection();
        isWalking = !isMidAir && player.actMove.x != 0 && !hitManager.IsPunching;
        animator.SetBool(IsWalkingHash, isWalking);
        isMidAir = !player.IsGrounded;
        animator.SetBool(IsMidAirHash, isMidAir);
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
