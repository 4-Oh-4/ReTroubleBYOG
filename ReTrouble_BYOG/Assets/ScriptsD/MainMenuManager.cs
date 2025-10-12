using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class MainMenuManager : MonoBehaviour
{
    // Make sure your levels are in the Build Settings!
    // MainMenu should be index 0.
    // LevelSelect should be index 1.
    // Level 1 should be index 2.
    [Header("Screen References")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject levelSelectScreen;

    public void PlayGame()
    {
        // Loads the first playable level (assuming it's at build index 2)
        SceneManager.LoadScene(2);
    }

    public void OpenSettings()
    {
        // TODO: Add logic to open a settings panel
        Debug.Log("Settings button clicked!");
    }

    public void OpenLevelSelect()
    {
        mainMenuScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // This only works in a built game, not the editor.
    }

    public void BackToMainMenu()
    {
        mainMenuScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
    }
}