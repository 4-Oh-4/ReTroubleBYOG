using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    // NEW: A public field to set in the Inspector
    [Header("Level Settings")]
    [Tooltip("The level number this button should load (e.g., 1, 2, 3...).")]
    public int levelToLoad;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI levelText;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadLevel); // Call LoadLevel when clicked
    }

    // This runs in the editor, so you can see your changes immediately.
    private void OnValidate()
    {
        if (levelText != null)
        {
            levelText.text = levelToLoad.ToString();
        }
    }

    void LoadLevel()
    {
        // IMPORTANT: This assumes your first playable level is at Build Index 1.
        // (e.g., Build Settings: 0 = MainMenu, 1 = Level 1, 2 = Level 2, etc.)
        // If your first level is at Build Index 2, change this line to:
        // SceneManager.LoadScene(levelToLoad + 1);
        SceneManager.LoadScene(levelToLoad);
    }
}