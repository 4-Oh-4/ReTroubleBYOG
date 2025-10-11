using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HealthManager_A : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    public int currentHealth { get; private set; }
    [SerializeField] HealthBar bar;

    private Animator anim;
    private PlayerController_D playerController;
    private bool isDead = false; // Prevents taking damage after death


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController_D>();
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

        // 1. Trigger the death animation
        anim.SetTrigger("Die");

        // 2. Disable player controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // 3. Disable the player's collider so bubbles pass through
        GetComponent<Collider2D>().enabled = false;

        // 4. Start a coroutine to wait for the animation before showing "Game Over"
        StartCoroutine(GameOverSequence());
    }


    private IEnumerator GameOverSequence()
    {
        // Wait for 2 seconds (or the length of your death animation)
        yield return new WaitForSeconds(2f);

        // Now, show the Game Over screen or freeze the game
        Time.timeScale = 0f;
        Debug.Log("Game Over");
        // Here you would typically show a UI panel, e.g., gameOverPanel.SetActive(true);
    }

}
