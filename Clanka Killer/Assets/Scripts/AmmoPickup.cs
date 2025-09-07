using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAdd = 30;
    
    public bool isInRange = false;
    public Image interact;
    public AudioSource openSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange == true)
        {
            Gun gun = GameObject.Find("WeaponHolder").GetComponentInChildren<Gun>();
            gun.maxAmmo += ammoAdd;
            if (openSound != null)
                openSound.PlayOneShot(openSound.clip);
            StartCoroutine(Despawn());

        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        interact.gameObject.SetActive(true);
        isInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        interact.gameObject.SetActive(false);
        isInRange = false;
    }

    IEnumerator Despawn()
    {
        interact.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
