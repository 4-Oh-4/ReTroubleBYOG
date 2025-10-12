using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class HealthManager_A : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    public int currentHealth { get; private set; }
    [SerializeField] HealthBar bar;

    private Animator anim;
    private PlayerController_D playerController; AudioManager_A audioManager;

    private SpawnArrow_N spawnArrow;
    private Rigidbody2D rb;
    [SerializeField]private PlayerInput playerInput;
    private bool isDead = false; // Prevents taking damage after death

    [Header("Damage Effect")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material damageMaterial;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 1.5f; // How long the player is invincible
    [SerializeField] private float flickerSpeed = 0.1f;      // How fast the player sprite blinks

    private SpriteRenderer spriteRenderer; // Reference to the player's sprite
    private bool isInvincible = false;     // Flag to check if currently invincible
    private Material initialMaterial;

    [SerializeField] Collider2D col;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_A>();

        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController_D>();
        spawnArrow = GetComponent<SpawnArrow_N>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isInvincible) spriteRenderer.enabled = true;   
    }

    public void IncreaseHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            Debug.Log("Health increased , currentHealth = " + currentHealth);
            // Optional: Update UI here
        }
    }
    
    public void DecreaseHealth()
    {

        if (isInvincible || isDead) return;

        // Standard Health Logic
        if (bar != null) bar.descreaseHealth();
        else Debug.Log(("attach healthbar"));

        currentHealth -= 1;
        Debug.Log("Health decreased , currentHealth = " + currentHealth);
        // Optional: Update UI here




        if (currentHealth <= 0)
        {
            if (playerInput != null) {
                playerInput.enabled = false;
            }
            Die();
        }
        else
        {
            audioManager.PlaySFX(audioManager.damage);

            StartCoroutine(InvincibilityCoroutine());
        }
    }


    private void Die()
    {
        isDead = true;
        audioManager.PlaySFX(audioManager.dieSound);

        // --- NEW: STOP ALL MOVEMENT IMMEDIATELY ---
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        // 1. Trigger the death animation
        anim.SetTrigger("Die");
        
        // 2. Disable player controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // 3. Disable the player's collider so bubbles pass through
        GetComponent<Collider2D>().enabled = false;

        
    }


    public void OnDeathAnimationComplete()
    {

        Time.timeScale = 0f;
        Debug.Log("Game Over");

        // And now, make the player invisible.
        Destroy(gameObject);

        GameUIManager.Instance.ShowLoseScreen();
    }


    private IEnumerator InvincibilityCoroutine()
    {
        // 1. Set the invincibility flag to true
        isInvincible = true;
        rb.constraints=RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;
        if (col != null) col.isTrigger = true;
        initialMaterial = spriteRenderer.material;
        if (damageMaterial != null)
        {
            spriteRenderer.material = damageMaterial;
        }

        // 2. Start the flickering loop
        float timer = 0f;
        while (timer < invincibilityDuration)
        {
            // Toggle sprite visibility
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Wait for a short duration
            yield return new WaitForSeconds(flickerSpeed);

            // Increment the timer
            timer += flickerSpeed;
        }

        // 3. Loop is over, ensure the player is visible and no longer invincible
        spriteRenderer.enabled = true;
        isInvincible = false;
        if (col != null) col.isTrigger = false;
        rb.constraints=RigidbodyConstraints2D.FreezeRotation;

        if (defaultMaterial != null)
        {
            spriteRenderer.enabled = true;

            spriteRenderer.material = initialMaterial;
        }
    }
}
