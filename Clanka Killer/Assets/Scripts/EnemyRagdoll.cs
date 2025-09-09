using UnityEngine;
using UnityEngine.AI;

public class EnemyRagdoll : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private Collider rootCollider;

    void Start()
    {
        // Get all rigidbodies + colliders in children (bones)
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        // Get the root collider (used for hit detection when alive)
        rootCollider = GetComponent<Collider>();

        // Turn off ragdoll at the start
        SetRagdollActive(false);
    }

    public void Die()
    {
        // Turn off Animator + NavMeshAgent
        if (animator != null) animator.enabled = false;
        if (agent != null) agent.enabled = false;

        // Turn off root collider so it doesn’t conflict with ragdoll colliders
        if (rootCollider != null) rootCollider.enabled = false;

        // Enable ragdoll physics
        SetRagdollActive(true);
        Destroy(gameObject,10);
    }

    private void SetRagdollActive(bool active)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            if (rb != null)
                rb.isKinematic = !active;
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col != rootCollider) // don’t touch the root collider
                col.enabled = active;
        }
    }
}
