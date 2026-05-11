using UnityEngine;
using UnityEngine.InputSystem;

public class AccumulationManager : MonoBehaviour
{
    private InputAction inputActionMove;
    private InputAction inputActionAccum;
    void Start()
    {
        inputActionMove = InputSystem.actions.FindAction("Move");
        inputActionAccum = InputSystem.actions.FindAction("Accumulate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
