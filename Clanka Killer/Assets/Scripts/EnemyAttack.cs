using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;         // Distance to start attacking
    public float attackRate = 1f;          // Attacks per second
    public int attackDamage = 10;          // Damage per hit
    public float attackDelay = 0.3f;       // Delay between swing and damage

    private float nextAttackTime = 0f;
    private Transform player;
    private PlayerHealth playerHealth;     // Reference to player health script
    private NavMeshAgent agent;
    private Animator animator;             // Optional (for attack animations)
    private EnemyDamage enemyDamage; // enemy health

    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Stop moving to attack
            if(enemyDamage.currentHealth > 0)
                agent.isStopped = true;

            // Face the player
            Vector3 lookDir = (player.position - transform.position);
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);

            // Try attacking
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                if(enemyDamage.currentHealth > 0)
                    Attack();
            }
        }
        else
        {
            
            if(enemyDamage.currentHealth > 0)
                agent.isStopped = false;
                // Resume movement
        }
    }

    void Attack()
    {
        // Play attack animation if available
        if (animator != null)
            animator.SetTrigger("Attack");

        // Delay to match swing timing
        Invoke(nameof(DealDamage), attackDelay);
    }

    void DealDamage()
    {

        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
