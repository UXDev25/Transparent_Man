using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private static readonly int GameWonHash = Animator.StringToHash("gameWon");
    private PlayerManager player;
    private PlayerHitManager hitManager;
    private Animator animator;

    private bool isMidAir;
    private bool isWalking;
    private bool isStunned;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        hitManager = GetComponent<PlayerHitManager>();
    }

    void Update()
    {
        if (GameManager.Instance.PauseCharacter) 
        {
            animator.SetBool(IsWalkingHash, false);
            animator.SetBool(IsMidAirHash, false);
            animator.SetBool(IsStunnedHash, false);
            return;
        } 
        if (!player.IsStunned && player.CanPlay) FlipDirection();
        isWalking = !isMidAir && player.actMove.x != 0 && !hitManager.IsAttacking;
        animator.SetBool(IsWalkingHash, isWalking);
        isMidAir = !player.IsGrounded;
        animator.SetBool(IsMidAirHash, isMidAir);
        isStunned = player.IsStunned;
        animator.SetBool(IsStunnedHash, isStunned);
        animator.SetBool(GameWonHash, GameManager.Instance.GameWon);
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
