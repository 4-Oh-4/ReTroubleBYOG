using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class HealthManager_A : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    public int currentHealth { get; private set; }
    [SerializeField] HealthBar bar;

    private Animator anim;
    private PlayerController_D playerController;
    private Rigidbody2D rb;
    private bool isDead = false; // Prevents taking damage after death


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController_D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
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

        // If already dead, don't do anything
        if (isDead) return;
        if (bar != null) bar.descreaseHealth();
        else Debug.Log(("attach healthbar"));
        currentHealth -= 1;
        Debug.Log("Health decreased , currentHealth = " + currentHealth);
        // Optional: Update UI here


        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        isDead = true;

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
        gameObject.SetActive(false);

        GameUIManager.Instance.ShowLoseScreen();
    }

}
