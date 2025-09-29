using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{
    [Header("Grenade")]
    public float fuseTime = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int maxDamage = 100;

    [Header("References")]
    public GameObject explosionPrefab;   // particle/sound prefab
    public LayerMask damageLayerMask;    // which layers take damage (enemies, destructibles)
    public LayerMask physicsLayerMask;   // which layers receive AddExplosionForce (optional)

    private bool exploded = false;

    public void StartFuse()
    {
        // start fuse coroutine
        StartCoroutine(FuseAndExplode());
    }

    IEnumerator FuseAndExplode()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    public void Explode()
    {
        if (exploded) return;
        exploded = true;

        // Spawn explosion VFX
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Damage and force
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);
        foreach (var col in colliders)
        {
            Debug.Log("Hit" + col.name);
            // Damage enemies or objects using a health script
            var enemy = col.GetComponentInParent<EnemyDamage>(); // or your health script
            if (enemy != null)
            {
                // Damage falloff: linear
                float dist = Vector3.Distance(transform.position, col.transform.position);
                float t = Mathf.Clamp01(dist / explosionRadius);
                int damage = Mathf.RoundToInt(Mathf.Lerp(maxDamage, 0, t));
                enemy.TakeDamage(damage);
            }

            // Add explosion force if the object has a rigidbody and is in physicsLayerMask
            var rb = col.attachedRigidbody;
            if (rb != null && (physicsLayerMask == (physicsLayerMask | (1 << col.gameObject.layer))))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        // Optionally remove grenade model (hide) before destroying to avoid collisions
        Destroy(gameObject);
    }

    // Optional: immediate explosion on collision
    void OnCollisionEnter(Collision collision)
    {
        // If you want grenade to explode on impact, uncomment:

        if (collision.gameObject.CompareTag("Enemy"))
            Explode();
    }
}
