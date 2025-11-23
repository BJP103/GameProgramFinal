using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyShooter : MonoBehaviour
{
    [Header("Muzzle Flash")]
    public GameObject muzzleFlashPrefab;
    public float muzzleFlashDuration = 0.05f;

    [Header("References")]
    public Transform player;
    public Transform modelToRotate;
    public Transform firePoint;

    [Header("Shooting (Hitscan)")]
    public float shootRange = 25f;
    public float fireRate = 1f;
    public int damage = 15;
    [Tooltip("Degrees of random spread (0 = perfectly accurate)")]
    public float spreadAngle = 1f;
    public LayerMask layerMask = ~0;              // what the raycast can hit (default: Everything)
    public float impactForce = 5f;
    public GameObject impactPrefab;               // optional small hit effect

    [Header("Audio")]
    public AudioSource shootAudioSource;
    public AudioClip shootSound;

    [Header("Rotation")]
    public float turnSpeed = 8f;
    public Vector3 rotationOffset;

    [Header("Debug")]
    public bool debugLogs = false;
    public bool drawDebugRay = false;
    public Color debugRayColor = Color.yellow;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextShotTime = 0f;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // prevent NavMeshAgent from overwriting rotation
        if (agent != null) agent.updateRotation = false;

        if (modelToRotate == null) modelToRotate = transform;

        if (shootAudioSource == null) shootAudioSource = GetComponent<AudioSource>();

        if (animator != null) animator.applyRootMotion = false;
    }

    void Update()
    {
        if (firePoint != null)
        {
            Debug.DrawRay(firePoint.position, firePoint.forward * 0.3f, Color.red);
        }
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= shootRange)
        {
            if (agent != null) agent.isStopped = true;

            // rotate model
            RotateTowardPlayer();

            // shoot if cooldown passed
            if (Time.time >= nextShotTime)
            {
                nextShotTime = Time.time + (1f / fireRate);
                ShootHitscan();
            }
        }
        else
        {
            if (agent != null)
            {
                agent.isStopped = false;
                if (agent.isOnNavMesh) agent.SetDestination(player.position);
            }
        }
    }

    void RotateTowardPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
        Quaternion finalRot = targetRot * Quaternion.Euler(rotationOffset);

        modelToRotate.rotation = Quaternion.Slerp(
            modelToRotate.rotation,
            finalRot,
            Time.deltaTime * turnSpeed
        );
    }
    void SpawnMuzzleFlash()
    {
        if (muzzleFlashPrefab == null || firePoint == null)
            return;

        GameObject flash = Instantiate(
        muzzleFlashPrefab,
        firePoint.position,
        firePoint.rotation * Quaternion.Euler(0, 0, 90f), // adjust if needed
        firePoint
       );


        Destroy(flash, muzzleFlashDuration);
    }


    void ShootHitscan()
    {
        //if (animator != null) animator.SetTrigger("Shoot");

        if (shootSound != null && shootAudioSource != null)
            shootAudioSource.PlayOneShot(shootSound);

        SpawnMuzzleFlash();

        if (firePoint == null)
        {
            if (debugLogs) Debug.LogWarning("EnemyShooter_Raycast: no firePoint assigned.");
            return;
        }

        // Calculate spread
        Vector3 forward = firePoint.forward;
        if (spreadAngle > 0f)
        {
            float half = spreadAngle * 0.5f;
            float yaw = Random.Range(-half, half);
            float pitch = Random.Range(-half, half);
            Quaternion spreadRot = Quaternion.Euler(pitch, yaw, 0f);
            forward = spreadRot * forward;
        }

        Vector3 origin = firePoint.position;
        RaycastHit hit;
        if (Physics.Raycast(origin, forward, out hit, shootRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (debugLogs) Debug.Log($"Enemy hit: {hit.collider.name}");

            // Try to apply damage: look for PlayerHealth on hit or parent
            var playerHealth = hit.collider.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                // Generic damage interface: try EnemyHealth as example (if you want)
                //var enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
                //if (enemyHealth != null)
                    //enemyHealth.TakeDamage(damage);
            }

            // Apply impact force if there's a rigidbody
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(forward * impactForce, hit.point, ForceMode.Impulse);
            }

            // Spawn impact effect
            if (impactPrefab != null)
            {
                Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        else
        {
            if (debugLogs) Debug.Log("Enemy shot but hit nothing.");
        }

        if (drawDebugRay)
        {
            Debug.DrawRay(origin, forward * shootRange, debugRayColor, 1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
