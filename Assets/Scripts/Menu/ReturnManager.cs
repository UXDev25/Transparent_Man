using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject return_Panel;
    [SerializeField] private GameObject black_Panel;

    private Animator fundido_A_Negro;


    void Start()
    {
        fundido_A_Negro = black_Panel.GetComponent<Animator>();
        black_Panel.SetActive(false);
        return_Panel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.GameWon) return_Panel.SetActive(true);
    }

    // -- Hacer el metodo con retraso --
    public void Play_Method(string method)
    {
        black_Panel.SetActive(true);
        Debug.Log("Method");
        fundido_A_Negro.SetTrigger("OpenAndClose");
        Invoke(method, 0.7f);
    }

    // -- Return --
    public void Return_Button()
    {
        Debug.Log("MainMenu");
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void Yes_exit()
    {
        Application.Quit();
    }

}
