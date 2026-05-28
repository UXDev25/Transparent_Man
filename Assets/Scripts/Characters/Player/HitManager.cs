using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitManager : MonoBehaviour
{
    private PlayerManager _playerManager;
    private Rigidbody2D _rb;
    private InputAction inputActionPunch;
    private GameObject _hitBoxOne;
    public bool IsPunching { get; private set; } = false;
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
        if (inputActionPunch.WasPressedThisFrame() && !IsPunching && _playerManager.IsGrounded)
        {
            _rb.linearVelocityX = 0;
            _playerManager.SetCanPlay(false);
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
            _hitBoxOne.SetActive(true);
            _rb.AddForce(new Vector2(transform.localScale.x * _playerManager.data.punchForwardForce, transform.position.y), ForceMode2D.Impulse);
        }
    }

    private void FinishCombo() 
    {
        _hitBoxOne.SetActive(false);
        IsPunching = false;
        PunchCounter = 0;
    }
    private void FinishAnimation()
    {
        _playerManager.SetCanPlay(true);
    }
}
