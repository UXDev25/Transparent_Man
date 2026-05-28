using UnityEngine;

public class EnemyManager : EntityManager
{

    protected void FlipDirection(bool enemyCondition)
    {
        transform.localScale = enemyCondition ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }
}
