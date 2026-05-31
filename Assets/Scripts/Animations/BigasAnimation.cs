using UnityEngine;

public class BigasAnimation : MonoBehaviour
{
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsStompingHash = Animator.StringToHash("isStomping");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private BigasManager bigas;
    private Animator animator;

    private bool isMidAir;
    private bool isWalking;
    private bool isStunned;
    private bool isJumping;
    private bool isStomping;

    void Start()
    {
        bigas = GetComponent<BigasManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isMidAir = false;   
        animator.SetBool(IsMidAirHash, isMidAir);
        isWalking = !isStunned;
        animator.SetBool(IsWalkingHash, isWalking);
        isStunned = bigas.IsStunned;
        animator.SetBool(IsStunnedHash, isStunned);
        isStomping = bigas.IsStomping;
        animator.SetBool(IsStompingHash, isStomping);
        isJumping = bigas.IsJumping;
        animator.SetBool(IsJumpingHash, isJumping);
    }
}
