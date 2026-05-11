using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject mainMenu_Panel;
    [SerializeField] private GameObject options_Panel;
    [SerializeField] private GameObject credits_Panel;
    [SerializeField] private GameObject exit_Panel;

    private Animator fundido_A_Negro;


    void Start()
    {
        fundido_A_Negro = GetComponent<Animator>();

        mainMenu_Panel.SetActive(true);
        options_Panel.SetActive(false);
        credits_Panel.SetActive(false);
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

    // -- Opciones --
    public void Options_Button()
    {
        mainMenu_Panel.SetActive(false);
        options_Panel.SetActive(true);
        credits_Panel.SetActive(false);
    }

    // -- Creditos --
    public void Credits_Button()
    {
        mainMenu_Panel.SetActive(false);
        options_Panel.SetActive(false);
        credits_Panel.SetActive(true);
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
        options_Panel.SetActive(false);
        credits_Panel.SetActive(false);
    }
}
