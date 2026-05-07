using System;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CinemachineCamera _vcam;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawn;
    public CinemachineCamera VCam => _vcam; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
