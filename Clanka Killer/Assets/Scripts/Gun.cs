using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 10f;
    public int currentAmmo = 30;
    public int maxAmmo = 120;
    public int magSize = 30;

    public Light muzzleLight;   // drag in your muzzle flash light
    public float lightDuration = 0.03f; // how long it flashes


    [Header("References")]
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;
    public AudioSource gunAudio;
    public AudioSource gunReloadAudio;
    public Text currentAmmo_text;
    public Text maxAmmo_text;
    public GunRecoil recoil;  // reference
                              
    [Header("References")]
    public Transform camTransform;  // drag in your Main Camera

    [Header("Camera Recoil")]
    public float recoilX = 2f;       // vertical kick
    public float recoilY = 1f;       // horizontal sway
    public float returnSpeed = 5f;   // how fast it returns
    public float snappiness = 8f;    // how sharp the kick feels

    private Vector2 currentRotation;
    private Vector2 targetRotation;



    private float nextTimeToFire = 0f;

    void Update()
    {
        currentAmmo_text.text = currentAmmo.ToString();
        maxAmmo_text.text = maxAmmo.ToString();

        // Old Input System (if enabled in Project Settings > Player > Input Handling)
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && maxAmmo > 0 || currentAmmo <= 0 && maxAmmo > 0)
        {
            Reload();
        }

    }

    void LateUpdate()
    {
        // Smooth recoil recovery
        targetRotation = Vector2.Lerp(targetRotation, Vector2.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector2.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        // Apply recoil last, after MouseLook already set the camera rotation
        camTransform.localRotation *= Quaternion.Euler(-currentRotation.x, currentRotation.y, 0f);
    }


    void Shoot()
    {
        // Add random recoil when shooting
        targetRotation += new Vector2(
            recoilX,
            UnityEngine.Random.Range(-recoilY, recoilY)
        );


        //Subtract one from currentAmmo
        Debug.Log("Curent Ammo:" + currentAmmo);
        currentAmmo --;

        //if(recoilCamera != null)
        //recoilCamera.ApplyRecoil();

        // Play muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Play Gunshot sound
        if (gunAudio != null)
            gunAudio.PlayOneShot(gunAudio.clip);

        if (recoil != null)
            recoil.ApplyRecoil();

        if (muzzleLight != null)
            StartCoroutine(MuzzleFlashLight());




        // Raycast from camera forward
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("We hit " + hit.transform.name);

            // If target has health
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Spawn hit effect
            if (hitEffectPrefab != null)
            {
                GameObject impactGO = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }

        
    }

    void Reload()
    {

        Debug.Log("Reloading");

        int ammoRemove = magSize - currentAmmo;

        ammoRemove = (maxAmmo - ammoRemove) >= 0 ? ammoRemove : maxAmmo;

        currentAmmo += ammoRemove;

        maxAmmo -= ammoRemove;

    }
    IEnumerator MuzzleFlashLight()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleLight.enabled = false;
    }

}
