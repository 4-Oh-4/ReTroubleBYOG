using UnityEngine;

public class FXManager : MonoBehaviour
{
    // Singleton pattern for easy access from any script
    public static FXManager Instance;

    [Header("Effect Prefabs")]
    [SerializeField] private GameObject popEffectPrefab;
    // You can also move the fwoosh effect here for better organization!
    // [SerializeField] private GameObject fwooshEffectPrefab;

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

    /// <summary>
    /// Spawns the bubble pop effect at a given position.
    /// </summary>
    public void SpawnPopEffect(Vector3 position)
    {
        if (popEffectPrefab != null)
        {
            Instantiate(popEffectPrefab, position, Quaternion.identity);
        }
    }
}