using UnityEngine;

public class RollonAnimation : MonoBehaviour
{
    private static readonly int IsRollingHash = Animator.StringToHash("isRolling");
    private static readonly int IsStunnedHash = Animator.StringToHash("isStunned");
    private RollonManager rollon;
    private Animator animator;

    private bool isRolling;
    private bool isStunned;

    void Start()
    {
        rollon = GetComponent<RollonManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isRolling = !isStunned;
        animator.SetBool(IsRollingHash, isRolling);
        isStunned = rollon.IsStunned;
        animator.SetBool(IsStunnedHash, isStunned);
    }
}
