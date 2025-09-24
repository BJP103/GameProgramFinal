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
    public AudioSource emptyMag;
    public Text currentAmmo_text;
    public Text maxAmmo_text;
    public Sprite weaponImg;
    public Image WeaponImage;
    public Animator weaponAnim;
    public AudioSource weaponReload;

    //public Text weaponName;
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

    private bool isReloading =false;


    private float nextTimeToFire = 0f;

    private void Start()
    {
    weaponAnim.enabled = false;
    }

    void Update()
    {

        currentAmmo_text.text = currentAmmo.ToString();
        maxAmmo_text.text = maxAmmo.ToString();
        
        Text WeaponNameTXT = GameObject.Find("WeaponName").GetComponent<Text>();

        WeaponNameTXT.text = gameObject.name;

        WeaponImage.sprite = weaponImg;

        if (isReloading) return;

        // Old Input System (if enabled in Project Settings > Player > Input Handling)
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weaponAnim.SetBool("IsShooting", true);
                Shoot();
                weaponAnim.SetBool("IsShooting", false);
            }

            if (maxAmmo == 0 && currentAmmo == 0)
            {
                if (emptyMag != null)
                    emptyMag.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && maxAmmo > 0 || currentAmmo <= 0 && maxAmmo > 0)
        {
            if(currentAmmo == magSize)
            {
                weaponAnim.enabled = false ;
            }
            else
            {
                weaponAnim.enabled = true;
                StartCoroutine(Reload());
            }


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

            // Check if hit enemy
            EnemyDamage enemy = hit.collider.GetComponent<EnemyDamage>();
            if (enemy != null)
            {
                enemy.TakeDamage(20); // apply damage
            }

            // Spawn hit effect
            if (hitEffectPrefab != null)
            {
                GameObject impactGO = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }

        
    }

    IEnumerator Reload()
    {
        isReloading = true;

        weaponAnim.SetBool("IsReloading", true);

        weaponReload.Play();

        yield return new WaitForSeconds(1f);

        weaponAnim.SetBool("IsReloading" ,false);
        weaponAnim.enabled = false;

        Debug.Log("Reloading");

        int ammoRemove = magSize - currentAmmo;

        ammoRemove = (maxAmmo - ammoRemove) >= 0 ? ammoRemove : maxAmmo;

        currentAmmo += ammoRemove;

        maxAmmo -= ammoRemove;

        isReloading = false;
    }
    IEnumerator MuzzleFlashLight()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleLight.enabled = false;
    }

}
