using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject creditsUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        creditsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenCredits()
    {
        pauseMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        creditsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void LoadCreditsScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
