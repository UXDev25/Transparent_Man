using UnityEngine;

public class EnemyManager : EntityManager
{
    protected bool isFacingRight = true;
    protected RaycastHit2D raycastHit;
    protected void FlipDirection(bool enemyCondition)
    {
        transform.localScale = enemyCondition ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }
}
