using System;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("References")]
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;
    public AudioSource gunAudio;
    public Text currentAmmo_text;
    public Text maxAmmo_text;
    public GunRecoil recoil;  // reference
    public CameraRecoil recoilCamera;


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

        // If you want New Input System, see note below 👇
    }

    void Shoot()
    {

        //Subtract one from currentAmmo
        Debug.Log("Curent Ammo:" + currentAmmo);
        currentAmmo --;

        if(recoilCamera != null)
        recoilCamera.ApplyRecoil();

        // Play muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Play Gunshot sound
        if (gunAudio != null)
            gunAudio.PlayOneShot(gunAudio.clip);

        if (recoil != null)
            recoil.ApplyRecoil();


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
}
