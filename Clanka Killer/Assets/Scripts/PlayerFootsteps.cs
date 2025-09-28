using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Footstep Clips")]
    public AudioClip[] defaultSteps;   // fallback footsteps
    public AudioClip[] woodSteps;
    public AudioClip[] grassSteps;
    public AudioClip[] stoneSteps;
    public AudioClip[] metalSteps;

    [Header("Step Timing")]
    public float baseStepDelay = 0.6f;  // walking speed step
    public float sprintStepMultiplier = 0.6f; // sprint faster = shorter delay
    public float walkStepMultiplier = 1.0f;   // normal walking delay
    public float crouchStepMultiplier = 1.4f; // crouching slower = longer delay

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
        if (controller.isGrounded && controller.velocity.magnitude > 0.2f)
        {
            stepCooldown -= Time.deltaTime;

            if (stepCooldown <= 0f)
            {
                PlayFootstep();
                stepCooldown = GetStepDelay();
            }
        }
    }

    float GetStepDelay()
    {
        float speed = controller.velocity.magnitude;

        // Example speed thresholds
        if (speed > 10f)       // Sprinting
            return baseStepDelay * sprintStepMultiplier;
        else if (speed > 2f)  // Walking
            return baseStepDelay * walkStepMultiplier;
        else                  // Crouching/slow move
            return baseStepDelay * crouchStepMultiplier;
    }

    void PlayFootstep()
    {
        AudioClip clip = null;

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 5f))
        {
            clip = GetFootstepClip(hit.collider.tag);
            Debug.Log("Hit surface: " + hit.collider.tag);
        }

        // If no hit or no surface clips found → fallback
        if (clip == null && defaultSteps.Length > 0)
        {
            clip = defaultSteps[Random.Range(0, defaultSteps.Length)];
            Debug.Log("Using default footstep");
        }

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    AudioClip GetFootstepClip(string tag)
    {
        AudioClip[] clips = defaultSteps;

        switch (tag)
        {
            case "Wood": clips = woodSteps; break;
            case "Grass": clips = grassSteps; break;
            case "Stone": clips = stoneSteps; break;
            case "Metal": clips = metalSteps; break;
        }

        if (clips != null && clips.Length > 0)
            return clips[Random.Range(0, clips.Length)];

        return null;
    }
}
