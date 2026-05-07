using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Vector2 _movementSpeed;
    [SerializeField] private float _movementSpeedMultiplier = 0.1f;
    [SerializeField] private float _baseMovementMult = 0;
    private Vector2 _offset;
    private Material _material;
    private GameObject _player;
    private Rigidbody2D _playerRb;

    private void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerRb = _player.GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        if (_player != null) 
        {
            _offset = ((_playerRb.linearVelocity.x + _baseMovementMult) * _movementSpeedMultiplier) * _movementSpeed * Time.deltaTime;
            _material.mainTextureOffset += _offset;
            transform.position = new Vector2(_player.transform.position.x, transform.position.y);
        }
    }
}
