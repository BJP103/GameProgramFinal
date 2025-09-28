using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource audioSource;            // Drag your AudioSource here
    public AudioClip[] footstepClips;          // Drag multiple footstep sounds here
    public float stepDelay = 0.5f;             // Time between footsteps

    private CharacterController controller;
    private float stepCooldown;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only play if moving and grounded
        if (controller.isGrounded && controller.velocity.magnitude > 0.2f)
        {
            stepCooldown -= Time.deltaTime;

            if (stepCooldown <= 0f)
            {
                PlayFootstep();
                stepCooldown = stepDelay;
            }
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int index = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[index]);
        }
    }
}
