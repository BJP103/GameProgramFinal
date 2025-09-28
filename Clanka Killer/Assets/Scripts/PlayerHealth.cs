using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;   // Drag your slider here in Inspector
    public Gradient healthGradient; // For color effect (optional)
    public Image fillImage;        // Reference to the fill image
    GameObject damageScreen;

    void Start()
    {
        damageScreen = GameObject.Find("DamageScreen");
        damageScreen.SetActive(false);
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if(currentHealth <= 25)
        {
            damageScreen.SetActive(true);
        }

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (fillImage != null && healthGradient != null)
            fillImage.color = healthGradient.Evaluate((float)currentHealth / maxHealth);
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Disable player, respawn, or trigger game over
    }
}
