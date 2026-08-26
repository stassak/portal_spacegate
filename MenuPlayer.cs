using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayer : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuScreen;

    [Header("Lose UI")]
    [SerializeField] private GameObject loseMenuScreen;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        GameState.IsGameOver = false;

        if (loseMenuScreen != null)
            loseMenuScreen.SetActive(false);

        if (pauseMenuScreen != null)
            pauseMenuScreen.SetActive(false);
    }

    void Update()
    {
        if (GameState.IsGameOver)
            return;

        // ESC key toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    //  START GAME
    public void PlayGame()
    {
        GameState.IsGameOver = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(1);
    }

    //  QUIT
    public void QuitGame()
    {
        Application.Quit();
    }

    // PAUSE (NO SCENE LOAD!)
    public void PauseGame()
    {
        if (GameState.IsGameOver) return;

        if (pauseMenuScreen != null)
        pauseMenuScreen.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    //  RESUME (NO SCENE LOAD!)
    public void ResumeGame()
    {
        if (pauseMenuScreen != null)
            pauseMenuScreen.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    //  RESTART SAME LEVEL
    public void RestartLevel()
    {
        GameState.IsGameOver = false;
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    //  MAIN MENU
    public void GoToMenuScene()
    {
        GameState.IsGameOver = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    //  NEXT LEVEL (ONLY WHEN BUTTON PRESSED)
    public void NextLevel()
    {
        GameState.IsGameOver = false;
        Time.timeScale = 1f;

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(next);
        }
        else
        {
            Debug.Log("No more levels ----------- Main Menu");
            SceneManager.LoadScene(0);
        }
    }

    //  PREVIOUS LEVEL
    public void PreviousLevel()
    {
        GameState.IsGameOver = false;
        Time.timeScale = 1f;

        int current = SceneManager.GetActiveScene().buildIndex;
        int prev = current - 1;

        if (prev > 0)
        {
            SceneManager.LoadScene(prev);
        }
        else
        {
            Debug.Log("Already first level to Main Menu");
            SceneManager.LoadScene(0);
        }
    }

    public void ShowLoseMenu()
    {
        GameState.IsGameOver = true;

        if (loseMenuScreen != null)
            loseMenuScreen.SetActive(true);

        Time.timeScale = 0f;
    }
}