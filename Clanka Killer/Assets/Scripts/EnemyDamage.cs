using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private EnemyRagdoll ragdoll; // if using ragdoll

    public EnemyHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        ragdoll = GetComponent<EnemyRagdoll>();
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining HP: " + currentHealth);
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");

        // Trigger ragdoll if available
        if (ragdoll != null)
            ragdoll.Die();

        // Or just destroy
        // Destroy(gameObject);
    }

    
}
