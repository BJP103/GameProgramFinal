using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject weaponPickup;
    public GameObject spawngunpoint;
    public Transform newpos;
    public Component Gun;
    public Image interact;
    public Image ammoCounter;

    public bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(weaponPickup, this.transform);
        transform.GetChild(0).gameObject.SetActive(false);

        //Gun = GetComponent<gun>();
        weaponPickup.SetActive(false);
        interact.gameObject.SetActive(false);
        ammoCounter.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange == true && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pick up " + gameObject.name);

            weaponPickup.transform.SetParent(newpos, false);
            interact.gameObject.SetActive(false);
            ammoCounter.gameObject.SetActive(true);
            Destroy(gameObject);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
        interact.gameObject.SetActive (true);
    }

    void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        interact.gameObject.SetActive(false);
    }
}
