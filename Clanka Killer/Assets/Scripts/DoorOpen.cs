using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator anim;
    public AudioSource openSound;  
    public AudioSource closeSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            openSound.Play();
            Debug.Log("Hit " + other.gameObject.name);
            anim.SetTrigger("Open");
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            closeSound.Play();
            anim.SetTrigger("Close");
    }
}
