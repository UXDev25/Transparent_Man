using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _enemyInstance;
    void Update()
    {
        if (GameManager.Instance.ResetedGame) 
        { 
            if (_enemyInstance == null) _enemyInstance = Instantiate(enemyPrefab, transform.position, transform.rotation);
        }
    }
}
