using UnityEngine;
using TMPro; // You must add this line to use TextMeshPro

public class LevelManager : MonoBehaviour
{
    // A Singleton allows other scripts to easily access this manager
    public static LevelManager Instance;

    [Header("Level Settings")]
    [Tooltip("The time limit for this specific level, in seconds.")]
    [SerializeField] private float levelDuration = 90f; // This is the value you can change per level!

    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    private int currentScore;
    private float timeLeft;
    private bool isLevelActive = true;

    private void Awake()
    {
        // Set up the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the level
        currentScore = 0;
        timeLeft = levelDuration;
        UpdateScoreUI();
        UpdateTimeUI();
    }

    void Update()
    {
        // If the level has ended (win/lose), stop the timer.
        if (!isLevelActive) return;

        if (timeLeft > 0)
        {
            // Decrease time and update the UI
            timeLeft -= Time.deltaTime;
            UpdateTimeUI();
        }
        else
        {
            // Time has run out
            timeLeft = 0;
            isLevelActive = false; // Stop the level
            UpdateTimeUI();

            Debug.Log("Time's Up! Level Failed.");
            // Call your GameUIManager to show the lose screen
            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowLoseScreen();
            }
        }
    }

    /// <summary>
    /// Adds points to the score. Call this from other scripts (e.g., when a bubble pops).
    /// </summary>
    public void AddScore(int points)
    {
        if (!isLevelActive) return; // Don't award points if the level is over

        currentScore += points;
        UpdateScoreUI();
    }

    // A private helper function to update the score text
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + currentScore;
        }
    }

    // A private helper function to update the timer text
    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            // We use CeilToInt to get a clean whole number (e.g., 59, 58, 57...)
            int secondsRemaining = Mathf.CeilToInt(timeLeft);
            timeText.text = "" + secondsRemaining;
        }
    }
}