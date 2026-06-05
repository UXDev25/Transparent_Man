using System;
using UnityEngine;

public class HitboxInfo : MonoBehaviour
{
    public Vector2 KnockBack { get; private set; }
    public void ChangeKnockBack(Vector2 knockback) 
    {
        KnockBack = knockback;
    }
}
