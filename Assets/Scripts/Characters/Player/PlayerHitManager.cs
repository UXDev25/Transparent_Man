using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHitManager : HitManager
{
    private PlayerManager _playerManager;
    private InputAction inputActionPunch;
    public int PunchCounter { get; private set; } = 0;
    private Animator _animator;

    void Start()
    {
        inputActionPunch = InputSystem.actions.FindAction("Punch");
        _animator = GetComponent<Animator>();
        _hitBoxOne = transform.GetChild(1).gameObject;
        _playerManager = GetComponent<PlayerManager>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Combo();
    }

    private void Combo() 
    { 
        if (inputActionPunch.WasPressedThisFrame() && !IsAttacking && _playerManager.IsGrounded)
        {
            _rb.linearVelocityX = 0;
            _playerManager.SetCanPlay(false);
            IsAttacking = true;
            _animator.SetTrigger(""+PunchCounter);
        }
    }

    private void StartCombo() 
    {
        IsAttacking = false;
        if (PunchCounter < 2)
        {
            Debug.Log($"{PunchCounter} {_hitBoxOne.GetComponent<HitboxInfo>().KnockBack}");
            PunchCounter++;
        }
    }

    protected override void ShowHitBox(int hitboxNumber)
    {
        base.ShowHitBox(hitboxNumber);
        ApplyKnockBack(hitboxNumber);
    }

    private void ApplyKnockBack(int punchCounter) 
    { 
        switch (punchCounter)
        {
            case <= 1:
                _hitBoxOne.GetComponent<HitboxInfo>().ChangeKnockBack(data.comboPunchKB);
                break;
            default: _hitBoxOne.GetComponent<HitboxInfo>().ChangeKnockBack(data.punchKB);
                break;
        }
    }

    private void FinishCombo() 
    {
        _hitBoxOne.SetActive(false);
        IsAttacking = false;
        PunchCounter = 0;
    }
    private void FinishAnimation()
    {
        _playerManager.SetCanPlay(true);
    }
}
