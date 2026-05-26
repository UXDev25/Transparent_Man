using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
public class HitManager : MonoBehaviour
{
    private PlayerManager _player;
    private InputAction inputActionPunch;
    public bool IsPunching { get; private set; } = false;
    public int PunchCounter { get; private set; } = 0;
    private float _betweeenPunchTimer = 0f;
    private Animator _animator;

    void Start()
    {
        inputActionPunch = InputSystem.actions.FindAction("Punch");
        _player = GetComponent<PlayerManager>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Combo();
    }

    private void Combo() 
    { 
        if (inputActionPunch.WasPressedThisFrame() && !IsPunching && _player.IsGrounded)
        {
            IsPunching = true;
            _animator.SetTrigger(""+PunchCounter);
        }
    }

    private void StartCombo() 
    {
        IsPunching = false;
        if (PunchCounter < 2)
        {
            PunchCounter++;
        }
    }

    private void FinishCombo() 
    {
        IsPunching = false;
        PunchCounter = 0;
    }
}
