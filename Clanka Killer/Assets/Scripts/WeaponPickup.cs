using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject weaponPickup;

    public GameObject spawngunpoint;

    public Transform newpos;

    public Component Gun;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(weaponPickup, this.transform);
        transform.GetChild(0).gameObject.SetActive(false);

        //Gun = GetComponent<gun>();
        weaponPickup.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter()
    {

        Debug.Log("Pick up " + gameObject.name);

        weaponPickup.transform.SetParent(newpos, false);

        Destroy(gameObject);
    }
}
