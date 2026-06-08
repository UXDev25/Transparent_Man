using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _enemyInstance;
    private void OnEnable()
    {
        GameManager.OnGameReset += SpawnEnemy;
    }

    private void OnDisable()
    {
        GameManager.OnGameReset -= SpawnEnemy;
    }

    private void SpawnEnemy()
    {
        if (_enemyInstance == null)
        {
            _enemyInstance = Instantiate(enemyPrefab, transform.position, transform.rotation);
        }
    }
}
