using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator anim;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {

            Debug.Log("Hit " + other.gameObject.name);
            anim.SetTrigger("Open");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            anim.SetTrigger("Close");
    }
}
