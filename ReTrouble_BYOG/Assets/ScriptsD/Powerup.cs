using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Use an enum to define the different types of powerups
    public enum PowerupType { Freeze, Shield }
    public PowerupType type;

    [Header("Despawn Settings")]
    [Tooltip("How long the powerup stays on the floor before disappearing.")]
    public float lifetimeOnFloor = 6.0f;
    private bool hasLanded = false;

    [Header("Blinking Effect")]
    [SerializeField] private float blinkDuration = 2.0f; // How long it blinks before disappearing
    [SerializeField] private float blinkSpeed = 0.2f;    // How fast it blinks (time for one flash on/off)
    private SpriteRenderer spriteRenderer; // Reference to the powerup's sprite renderer



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Powerup " + gameObject.name + " is missing a SpriteRenderer!", this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find the Power Manager in the scene
            PowerManger_N powerManager = FindObjectOfType<PowerManger_N>();
            if (powerManager != null)
            {
                // Tell the manager to activate the specific powerup type
                powerManager.ActivatePowerup(type);
            }

            // Destroy the powerup item after collection
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we've hit the floor and if the timer hasn't started yet
        if (collision.gameObject.CompareTag("Floor") && !hasLanded)
        {
            hasLanded = true; // Set the flag so this won't run again
            StartCoroutine(DespawnAfterTime());
        }
    }

    private IEnumerator DespawnAfterTime()
    {
        // First, wait for the majority of the powerup's lifetime
        yield return new WaitForSeconds(lifetimeOnFloor - blinkDuration);

        // --- Start Blinking ---
        if (spriteRenderer != null) // Only blink if we have a SpriteRenderer
        {
            float timer = 0f;
            while (timer < blinkDuration)
            {
                // Toggle visibility
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(blinkSpeed);
                timer += blinkSpeed;
            }
            spriteRenderer.enabled = true; // Ensure it's visible if the loop ends on hidden
        }
        // --- End Blinking ---

        // Destroy the powerup GameObject
        Destroy(gameObject);
    }
}