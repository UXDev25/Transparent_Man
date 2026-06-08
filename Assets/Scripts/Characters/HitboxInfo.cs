using System;
using UnityEngine;

public class HitboxInfo : MonoBehaviour
{
    public Vector2 KnockBack { get; private set; }

    private AudioManager _audioManager;
    void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void ChangeKnockBack(Vector2 knockback) 
    {
        KnockBack = knockback;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") 
        {
            _audioManager.PlayHitSFX();
        }
    }
}
