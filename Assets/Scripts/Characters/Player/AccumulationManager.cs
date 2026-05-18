using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

[RequireComponent(typeof(PlayerManager))]
public class AccumulationManager : MonoBehaviour
{
    public Data data;
    private InputAction inputActionMove;
    private InputAction inputActionJump;
    private PlayerManager _player;

    private float _counter = 0f;
    private float _timer = 0f;
    public bool IsAccumulating { get; private set; } = false;

    void Start()
    {
        inputActionMove = InputSystem.actions.FindAction("Move");
        inputActionJump = InputSystem.actions.FindAction("Jump");
        _player = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (_player.IsGrounded) HandleAccumulation();
    }

    public bool IsPressingDown() 
    {
        Vector2 movementDirection = inputActionMove.ReadValue<Vector2>();
        if (movementDirection.y < -0.1f) return true;
        return false;
    }

    private void HandleAccumulation()
    {
        if (IsPressingDown() && !_player.IsStunned || !_player.IsGrounded)
        {
            if (!IsAccumulating) StartAccumulation();

            UpdateAccumulation();
        }
        else if (_counter == 0 && IsAccumulating) 
        {
            EndAccumulation();
        }
    }

    private void StartAccumulation()
    {
        IsAccumulating = true;
        _counter = 0f;
        _timer = 0f;
    }

    private void UpdateAccumulation()
    {
        _timer += Time.deltaTime;
        if (inputActionJump.WasPressedThisFrame()) _counter += data.jumpMultAccum;
        if ((_counter >= data.maxJumpAccum || _timer >= data.maxTimeAccum)) EndAccumulation();
    }

    private void EndAccumulation()
    {
        Debug.Log($"Final accum value: {_counter}");
        IsAccumulating = false;
        _player.SetBufferCounter(data.jumpBufferTime);
        _player.Jump(_counter);
    }

}
