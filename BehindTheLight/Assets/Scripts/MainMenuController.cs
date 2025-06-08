using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnPause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        GameManager.instance.SetPaused();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
