using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private float currentHealth;   // use float for regen accuracy

    [Header("UI")]
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fillImage;

    [Header("Regen Settings")]
    public bool canRegen = true;
    public float regenDelay = 5f;       // seconds after damage before regen starts
    public float regenRate = 5f;       // HP per second
    private float lastDamageTime;       // time damage was last taken

    GameObject damageScreen;
    public AudioSource heartBeat;

    public GameManager gameManager;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        damageScreen = GameObject.Find("DamageScreen");
        
        damageScreen.SetActive(false);
        heartBeat.enabled = false;
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    void Update()
    {
        HandleRegen();
        
        if (currentHealth > 30)
        {
            damageScreen.SetActive(false);
            heartBeat.enabled = false;
        }

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        lastDamageTime = Time.time; // reset regen timer
        Debug.Log("Took damage. Current HP = " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 30)
        {
            damageScreen.SetActive(true);
            heartBeat.enabled = true;
            if (heartBeat.enabled == false)
            {
                heartBeat.PlayOneShot(heartBeat.clip);
            }
        }
        if (currentHealth <= 0)
        {
            gameManager.PlayerDied();
            //Debug.Log("Player Died!");
            Die();
        }
            
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    void HandleRegen()
    {
        if (!canRegen || currentHealth <= 0) return;

        if (Time.time >= lastDamageTime + regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime; // smooth regen
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            UpdateHealthUI();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (fillImage != null && healthGradient != null)
            fillImage.color = healthGradient.Evaluate(currentHealth / maxHealth);
    }

    void Die()
    {
        
        Debug.Log("Player Died!");

    }
}
