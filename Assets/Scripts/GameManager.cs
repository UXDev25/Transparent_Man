using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CinemachineCamera _vcam;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawn;

    [Header("Search Config")]
    [SerializeField] private string _targetScene = "SampleScene"; 
    [SerializeField] private string _tagSearch = "Boss";

    [Header("LevelEnd")]
    [SerializeField] private float _finalHitTime = 2f;
    [SerializeField] private float _waitBeforeWin = 3f;
    public bool GameWon { get; private set; } 
    public CinemachineCamera VCam => _vcam;

    public bool PauseCharacter { get; private set; } = false;
    public EntityManager Boss { get; private set; }

    public void ResetGame() 
    {
        GameWon = false;
    }

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

    private void Update()
    {
        if (Boss.IsDead == EDeathState.Dying) Win();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _targetScene)
        {
            SearchForGameObject();
        }
    }

    private void SearchForGameObject()
    {
        // Busquem l'objecte pel tag
        GameObject foundObject = GameObject.FindWithTag(_tagSearch);

        // 3. VALIDACIÓ I SEGURETAT (Per assegurar-nos al 100%)
        if (foundObject != null)
        {
            Boss = foundObject.GetComponentInChildren<EntityManager>();
            Debug.Log($"[GameManager] Èxit: S'ha trobat i assignat l'objecte amb el tag '{_tagSearch}' a l'escena '{_targetScene}'.");
        }
        else
        {
            // Si és null, fem saltar un error vermell a la consola per saber exactament què ha anat malament
            Debug.LogError($"[GameManager] ERROR: No s'ha trobat cap objecte ACTIU amb el tag '{_tagSearch}' a l'escena '{_targetScene}'.");
        }
    }
    private void Win() 
    {
        PauseCharacter = true;
        StartCoroutine(FinalHitRoutine());
    }

    private IEnumerator FinalHitRoutine() 
    {
        Time.timeScale = 0.25f;
        yield return new WaitForSecondsRealtime(_finalHitTime);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(_waitBeforeWin);
        GameWon = true;
    }
}
