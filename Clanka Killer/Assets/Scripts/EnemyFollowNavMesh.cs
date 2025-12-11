using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowNavMesh : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    [Header("Detection Settings")]
    public float detectionRange = 10f;   // Enemy starts chasing
    public float stopRange = 12f;        // Enemy stops chasing (optional hysteresis)
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Snap to NavMesh on start
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Begin chasing if within detection range
        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
        }

        // Stop chasing if outside stop range
        if (isChasing && distance > stopRange)
        {
            isChasing = false;
            agent.ResetPath();
        }

        // Chase player
        if (isChasing)
        {
            agent.SetDestination(player.position);
        }
    }

    // Draw detection radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = new Color(1f, 0.5f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}
