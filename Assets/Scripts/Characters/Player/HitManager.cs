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

    private bool _canPunch = true;
    private float _betweeenPunchTimer = 0f;

    void Start()
    {
        inputActionPunch = InputSystem.actions.FindAction("Punch");
        _player = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (inputActionPunch.WasPressedThisFrame() && _canPunch && _player.IsGrounded)
        {
            IsPunching = true;
            PunchCounter++;
            if (PunchCounter <= 2) SpawnHitBox();
            _betweeenPunchTimer = 0.5f;
            if (PunchCounter > 2) return;
            if (PunchCounter == 1) StartCoroutine(PunchingRoutine());
        }
        else { _betweeenPunchTimer -= Time.deltaTime; }

        if (_betweeenPunchTimer > 0) 
        {
            _betweeenPunchTimer = 0;
        }
        
    }

    private IEnumerator PunchingBetween()
    {
        yield return new WaitForSeconds(1f);
        IsPunching = false;
        PunchCounter = 0;
        _canPunch = false;
        StartCoroutine(PunchingCooldown());
    }

    private IEnumerator PunchingRoutine() 
    {
        yield return new WaitForSeconds(1f);
        IsPunching = false;
        PunchCounter = 0;
        _canPunch = false;
        StartCoroutine(PunchingCooldown());
    }

    private IEnumerator PunchingCooldown()
    {
        Debug.Log("entering punch cooldown");
        yield return new WaitForSeconds(0.5f);
        _canPunch = true;
        Debug.Log(_canPunch);
    }

    private void SpawnHitBox() 
    { 
        Debug.Log("PUNCH! " + PunchCounter);
    }
}
