using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private EnemyRagdoll ragdoll; // if using ragdoll

    public EnemyHealthBar healthBar;
    GameObject player;
    EnemyShooter enemyShooter;

    void Start()
    {
        currentHealth = maxHealth;
        ragdoll = GetComponent<EnemyRagdoll>();
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        player = GameObject.Find("Player");
        enemyShooter = GetComponent<EnemyShooter>();
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
        
        if(enemyShooter != null)
            enemyShooter.enabled = false;

        // Or just destroy
        // Destroy(gameObject);
    }
   /* private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touching" + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }*/
}
