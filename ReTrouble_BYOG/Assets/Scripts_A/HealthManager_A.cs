using UnityEngine;

public class HealthManager_A : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

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
            Debug.Log("Game Over");
        }
    }
}
