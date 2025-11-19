using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyShooter : MonoBehaviour
{
    [Header("Targets & Prefabs")]
    public Transform player;                 // If null, will auto-find tag "Player"
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Shooting")]
    public float shootRange = 20f;
    public float fireRate = 1f;              // shots per second
    public int damage = 10;

    [Header("Movement / Rotation")]
    public float turnSpeed = 8f;             // how fast to rotate to face the player
    public Transform modelToRotate;          // if null, rotates the root transform

    [Header("Debug")]
    public bool debugLogs = true;
    public Color gizmoColor = Color.red;

    private NavMeshAgent agent;
    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        // Auto-assign player if not set
        if (player == null)
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) player = playerGO.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // important: let agent move but not update rotation
        agent.updatePosition = true;
        agent.updateRotation = false;

        if (animator != null)
            animator.applyRootMotion = false; // so animator doesn't hijack rotation

        if (modelToRotate == null)
            modelToRotate = this.transform; // rotate root by default

        if (debugLogs) Debug.Log($"[EnemyShooterFixed] Start. Player: {(player ? player.name : "null")}");
    }

    void Update()
    {
        if (player == null)
        {
            if (debugLogs) Debug.LogWarning("[EnemyShooterFixed] Player transform missing!");
            return;
        }

        // If agent not on navmesh, try to sample nearest navmesh point once
        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                if (debugLogs) Debug.Log("[EnemyShooterFixed] Snapped enemy to NavMesh.");
            }
            else
            {
                if (debugLogs) Debug.LogWarning("[EnemyShooterFixed] Agent is not on NavMesh and SamplePosition failed.");
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootRange)
        {
            // stop moving and face player
            agent.isStopped = true;

            // rotate will be applied in LateUpdate() to ensure it happens after agents update
            // shoot
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
        }
        else
        {
            // chase player
            if (!agent.isOnNavMesh) return;
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void LateUpdate()
    {
        // Apply rotation after NavMeshAgent moved to prevent it being overridden
        if (player == null) return;

        Vector3 dir = (player.position - transform.position);
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            // rotate ONLY the model, not the root
            modelToRotate.rotation = Quaternion.Slerp(
                modelToRotate.rotation,
                targetRot,
                Time.deltaTime * turnSpeed
            );
        }
    }

    void Shoot()
    {
        if (debugLogs) Debug.Log($"[EnemyShooterFixed] Shooting at player. Distance: {Vector3.Distance(transform.position, player.position):F2}");

        //if (animator != null)
            //animator.SetTrigger("Shoot");

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            // If bullet uses script and velocity, it will handle hit/damage
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, shootRange);
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up * 1.5f, player.position + Vector3.up * 1.5f);
        }
    }
}
