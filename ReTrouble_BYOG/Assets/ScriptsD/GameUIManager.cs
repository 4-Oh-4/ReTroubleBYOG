using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    // A static instance to be accessible from other scripts (Singleton)
    public static GameUIManager Instance;

    private bool isPaused = false;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Hide all panels at the very start
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void Start()
    {
        // Ensure the game is running and cursor is locked at the start of the level
        Time.timeScale = 1f;
        LockCursor();
    }

    private void Update()
    {
        // Listen for the Escape key to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Don't allow pausing if the win/lose screen is already up
            if (winPanel.activeInHierarchy || losePanel.activeInHierarchy)
            {
                return;
            }

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // --- Core UI Control Functions ---
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freezes game time
        pausePanel.SetActive(true);
        UnlockCursor();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes game time
        pausePanel.SetActive(false);
        LockCursor();
    }

    public void ShowWinScreen()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        UnlockCursor();
    }

    public void ShowLoseScreen()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        UnlockCursor();
    }

    // --- Button Functions ---
    public void RetryLevel()
    {
        Time.timeScale = 1f; // Unfreeze time before changing scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        // Optional: Check if the next level exists before trying to load it
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("You beat the last level! Returning to Main Menu.");
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Assumes Main Menu is at index 0
    }

    // --- Cursor Management ---
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}