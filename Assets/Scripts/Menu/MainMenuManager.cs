using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject mainMenu_Panel;
    [SerializeField] private GameObject exit_Panel;

    private Animator fundido_A_Negro;


    void Start()
    {
        fundido_A_Negro = GetComponent<Animator>();
        GameManager.Instance.ResetGame();
        mainMenu_Panel.SetActive(true);
        exit_Panel.SetActive(false);
    }

    void Update()
    {

    }

    // -- Hacer el metodo con retraso --
    public void Play_Method(string method)
    {
        Debug.Log("Method");
        fundido_A_Negro.SetTrigger("OpenAndClose");
        Invoke(method, 0.45f);
    }

    // -- Play --
    public void Play_Button()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("SampleScene");
    }


    // -- Slair del juego --
    public void Exit_Button()
    {
        exit_Panel.SetActive(true);
    }

    public void Yes_exit()
    {
        Application.Quit();
    }

    public void No_exit()
    {
        exit_Panel.SetActive(false);
    }

    // -- Volver al MainMenu --
    public void Back_Button()
    {
        mainMenu_Panel.SetActive(true);
    }
}
