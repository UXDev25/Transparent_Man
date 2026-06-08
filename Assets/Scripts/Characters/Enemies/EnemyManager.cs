using UnityEngine;

public class EnemyManager : EntityManager
{
    protected bool isFacingRight = true;
    protected RaycastHit2D raycastHit;
    [SerializeField] float enemySize = 1;
    protected void FlipDirection(bool enemyCondition)
    {
        transform.localScale = enemyCondition ? new Vector3(enemySize, enemySize, 1) : new Vector3(-enemySize, enemySize, 1);
    }
}
