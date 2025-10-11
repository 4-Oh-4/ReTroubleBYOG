using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class HealthManager_A : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    public int currentHealth { get; private set; }

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
        }
    }
    
    public void DecreaseHealth()
    {
        currentHealth -= 1;
        Debug.Log("Health decreased , currentHealth = " + currentHealth);
        if (currentHealth <= 0)
        {
            Time.timeScale = 0f;
            Debug.Log("Game Over");
        }
    }
}
