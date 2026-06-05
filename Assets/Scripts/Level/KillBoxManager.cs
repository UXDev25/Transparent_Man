using Unity.Cinemachine;
using UnityEngine;

public class KillBoxManager : MonoBehaviour
{
    
    [SerializeField] private Data _data;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collisioned");
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Dying");
            if (collision.TryGetComponent(out EntityManager entity)) 
            {
                entity.Die();
            }
        }
    }
}
