using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform modelToRotate;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Audio")]
    public AudioSource shootAudioSource;
    public AudioClip shootSound;

    [Header("Settings")]
    public float shootRange = 15f;
    public float fireRate = 1f;
    public float turnSpeed = 8f;
    public Vector3 rotationOffset;

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

        agent.updateRotation = false;

        if (modelToRotate == null)
            modelToRotate = transform;

        if (shootAudioSource == null)
            shootAudioSource = GetComponent<AudioSource>();

        if (animator != null)
            animator.applyRootMotion = false;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= shootRange)
        {
            agent.isStopped = true;
            RotateTowardPlayer();

            if (Time.time >= nextShotTime)
            {
                Shoot();
                nextShotTime = Time.time + (1f / fireRate);
            }
        }
        else
        {
            agent.isStopped = false;
            if (agent.isOnNavMesh)
                agent.SetDestination(player.position);
        }
    }

    void RotateTowardPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion finalRot = targetRot * Quaternion.Euler(rotationOffset);

        modelToRotate.rotation = Quaternion.Slerp(
            modelToRotate.rotation,
            finalRot,
            Time.deltaTime * turnSpeed
        );
    }

    void Shoot()
    {
        if (animator != null)
            animator.SetTrigger("Shoot");

        if (shootSound != null && shootAudioSource != null)
            shootAudioSource.PlayOneShot(shootSound);

        if (bulletPrefab != null && firePoint != null)
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
