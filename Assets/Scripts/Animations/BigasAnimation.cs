using UnityEngine;

public class BigasAnimation : MonoBehaviour
{
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int IsMidAirHash = Animator.StringToHash("isMidAir");
    private static readonly int IsStompingHash = Animator.StringToHash("isStomping");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private static readonly int StateHash = Animator.StringToHash("state");
    private BigasManager bigas;
    private Animator animator;

    private bool isMidAir;
    private bool isWalking;
    private bool isStunned;
    private bool isJumping;
    private bool isStomping;
    private int state;

    void Start()
    {
        bigas = GetComponent<BigasManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isMidAir = !bigas.IsGrounded;   
        animator.SetBool(IsMidAirHash, isMidAir);
        isStunned = bigas.IsStunned;
        animator.SetBool(IsStunnedHash, isStunned);
        state = (int)bigas.State;
        animator.SetInteger(StateHash, state);
    }
}
