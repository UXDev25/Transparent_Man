using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    void Update()
    {
        if (GameManager.Instance.ResetedGame) 
        { 
            Instantiate(enemyPrefab);
            Debug.Log("Enemy Spawned");
        }
    }
}
