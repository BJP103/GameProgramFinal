using UnityEngine;

public class EnemyFootsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footsteps;
    public float stepInterval = 0.5f;

    private float timer = 0f;
    private Vector3 lastPos;

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPos);

        // Only play while moving
        if (distanceMoved > 0.01f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
                timer = stepInterval;
            }
        }

        lastPos = transform.position;
    }
}
