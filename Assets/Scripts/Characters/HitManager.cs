using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitManager : MonoBehaviour
{
    protected Data data;
    protected Rigidbody2D _rb;
    protected GameObject _hitBoxOne;
    public bool IsAttacking { get; protected set; } = false;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _hitBoxOne = transform.GetChild(1).gameObject;
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void HideHitBox() 
    {
        _rb.linearVelocityX = 0;
        _hitBoxOne.SetActive(false);
    }

    protected virtual void ShowHitBox(int hitboxNumber)
    {
        _rb.AddForce(new Vector2(transform.localScale.x * data.punchForwardForce, 0), ForceMode2D.Impulse);
        _hitBoxOne.SetActive(true);
    }
}
