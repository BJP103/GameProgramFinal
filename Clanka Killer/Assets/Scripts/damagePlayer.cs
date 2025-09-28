using UnityEngine;

public class damagePlayer : MonoBehaviour
{
    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touching" + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>().TakeDamage(20);
        }
    }

}
