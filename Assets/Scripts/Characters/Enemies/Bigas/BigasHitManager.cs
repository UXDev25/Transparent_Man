using UnityEngine;

public class BigasHitManager : MonoBehaviour, IHitSystem
{
    private BigasManager _bigasManager;
    private GameObject _hitBoxOne;
    private Rigidbody2D _rb;
    [SerializeField] private Data data;
    public bool IsAttacking { get; set; } = false;
    void Start()
    {
        _bigasManager = GetComponent<BigasManager>();
        _rb = GetComponent<Rigidbody2D>();
        _hitBoxOne = transform.GetChild(1).gameObject;
    }

    public void ShowHitBox(int hitboxNumber)
    {
        _rb.linearVelocityX = 0;
        _rb.AddForce(new Vector2(-transform.localScale.x * data.punchForwardForce, 0), ForceMode2D.Impulse);
        _hitBoxOne.SetActive(true);
    }

    public void HideHitBox()
    {
        _hitBoxOne.SetActive(false);
    }
}
