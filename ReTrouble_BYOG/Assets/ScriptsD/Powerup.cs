using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Powerup : MonoBehaviour
{
    // Use an enum to define the different types of powerups
    public enum PowerupType { Freeze, Shield }
    public PowerupType type;
    private Rigidbody2D rb;

    [Header("Hover & Despawn")]
    [Tooltip("The final Y-position where the powerup will stop and hover.")]
    public float hoverHeight = -3.5f;
    [Tooltip("How long the powerup stays while hovering before disappearing.")]
    public float hoverLifetime = 8.0f; // MODIFIED: Renamed for clarity

    [Header("Blinking Effect")]
    [SerializeField] private float blinkDuration = 2.0f; // How long it blinks before disappearing
    [SerializeField] private float blinkSpeed = 0.2f;    // How fast it blinks (time for one flash on/off)
    private SpriteRenderer spriteRenderer; // Reference to the powerup's sprite renderer
    private bool isHovering = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Powerup " + gameObject.name + " is missing a SpriteRenderer!", this);
        }
    }

    private void FixedUpdate()
    {
        // If we are not yet hovering and we have fallen to or below the target height...
        if (!isHovering && transform.position.y <= hoverHeight)
        {
            StartHovering();
        }
    }

    private void StartHovering()
    {
        isHovering = true;

        // Stop all physical movement
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        // Lock the position precisely at the hover height
        transform.position = new Vector3(transform.position.x, hoverHeight, transform.position.z);

        // Start the despawn timer now that it's hovering
        StartCoroutine(DespawnAfterTime());
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



    private IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(hoverLifetime - blinkDuration);

        // --- Blinking Logic (no changes here) ---
        if (spriteRenderer != null)
        {
            float timer = 0f;
            while (timer < blinkDuration)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(blinkSpeed);
                timer += blinkSpeed;
            }
            spriteRenderer.enabled = true;
        }

        Destroy(gameObject);
    }
}