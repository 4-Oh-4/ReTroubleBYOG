using System.Collections;
using UnityEngine;
using TMPro; // Make sure to add this for TextMeshPro

public class PowerManger_N : MonoBehaviour
{
    [Header("State")]
    public bool hasPowerUp = false; // You can use this to track if ANY powerup is active if needed

    [Header("Powerup Prefabs")]
    [SerializeField] private GameObject freezePowerupPrefab;
    [SerializeField] private GameObject shieldPowerupPrefab;
    private GameObject[] powerupPrefabs;

    [Header("Freeze Settings")]
    [SerializeField] private float freezeDuration = 5.0f;
    [SerializeField] private TextMeshProUGUI freezeTimerText;

    // Singleton pattern for easy access
    public static PowerManger_N Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the array of powerups for random spawning
        powerupPrefabs = new GameObject[] { freezePowerupPrefab, shieldPowerupPrefab };
        if (freezeTimerText != null)
        {
            freezeTimerText.gameObject.SetActive(false);
        }
    }

    // This is the main function called by the Powerup script
    public void ActivatePowerup(Powerup.PowerupType type)
    {
        if (type == Powerup.PowerupType.Freeze)
        {
            StartCoroutine(FreezeCoroutine());
        }
        else if (type == Powerup.PowerupType.Shield)
        {
            ActivateShield();
        }
    }

    // --- Freeze Logic ---
    private IEnumerator FreezeCoroutine()
    {
        // Find all bubbles
        BubbleHeightAdjustment_N[] bubbles = FindObjectsOfType<BubbleHeightAdjustment_N>();
        HeightAdjustmentFusion_N[] bubblesfusion = FindObjectsOfType<HeightAdjustmentFusion_N>();
        foreach (var bubble in bubbles)
        {
            bubble.Freeze();
        }
        foreach (var bubble in bubblesfusion) {
            bubble.Freeze();
        }
        // Handle the UI Timer
        freezeTimerText.gameObject.SetActive(true);
        float timeLeft = freezeDuration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            freezeTimerText.text = timeLeft.ToString("F1"); // Show one decimal place
            yield return null;
        }
        freezeTimerText.gameObject.SetActive(false);


        // Unfreeze all bubbles (find them again in case some were destroyed)
        bubbles = FindObjectsOfType<BubbleHeightAdjustment_N>();
        bubblesfusion = FindObjectsOfType<HeightAdjustmentFusion_N>();
        foreach (var bubble in bubbles)
        {
            if (bubble != null) // Check if bubble still exists
            {
                bubble.Unfreeze();
            }
        }
        foreach (var bubble in bubblesfusion) {
            if (bubble != null) // Check if bubble still exists
            {
                bubble.Unfreeze();
            }
        }
    }

    // --- Shield Logic ---
    private void ActivateShield()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController_D>().EnableShield();
        }
    }

    // --- Spawning Logic (called by bubbles) ---
    public void SpawnRandomPowerup(Vector3 position)
    {
        if (powerupPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomIndex], position, Quaternion.identity);
        }
    }
}