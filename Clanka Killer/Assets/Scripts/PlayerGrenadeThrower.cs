using UnityEngine;

public class PlayerGrenadeThrower : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab;
    public Transform spawnPoint;      // where grenade appears (Muzzle or hand)
    public float minThrowForce = 6f;
    public float maxThrowForce = 18f;
    public float maxChargeTime = 1.0f; // time to reach max throw power
    public int grenadeCount = 3;

    [Header("Fuse/Cook")]
    public float defaultFuseTime = 3f; // set on grenade instance (you can override for cook)

    private float chargeStartTime;
    private bool charging = false;

    void Update()
    {
        // Press and hold G to start charge, release to throw
        if (Input.GetKeyDown(KeyCode.G) && grenadeCount > 0)
        {
            charging = true;
            chargeStartTime = Time.time;
        }

        if (charging && Input.GetKeyUp(KeyCode.G))
        {
            float charge = Mathf.Clamp01((Time.time - chargeStartTime) / maxChargeTime);
            ThrowGrenade(charge);
            charging = false;
        }

        // Quick tap: if you want single press throw:
        // if (Input.GetKeyDown(KeyCode.G) && grenadeCount > 0) ThrowGrenade(0.5f);
    }

    void ThrowGrenade(float chargeNormalized)
    {
        if (grenadeCount <= 0) return;

        grenadeCount--;

        // instantiate grenade
        GameObject g = Instantiate(grenadePrefab, spawnPoint.position, spawnPoint.rotation);

        // set fuse (cook if holding)
        Grenade grenadeScript = g.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            // If you want cooking behavior: reduce fuse by time charged
            float cookedTime = Mathf.Lerp(0f, grenadeScript.fuseTime * 0.9f, chargeNormalized); // example
            grenadeScript.fuseTime = Mathf.Max(0.1f, grenadeScript.fuseTime - (Time.time - chargeStartTime));
            // start its fuse
            grenadeScript.StartFuse();
        }

        // apply throw force
        Rigidbody rb = g.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float force = Mathf.Lerp(minThrowForce, maxThrowForce, chargeNormalized);
            // direction: forward + small upward arc
            Vector3 dir = spawnPoint.forward + spawnPoint.up * 0.2f;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(dir.normalized * force, ForceMode.VelocityChange);
        }
    }
}
