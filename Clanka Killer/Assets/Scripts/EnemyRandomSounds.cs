using UnityEngine;

public class EnemyRandomSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] randomClips;

    public float minDelay = 2f;
    public float maxDelay = 6f;

    float timer;

    void Start()
    {
        SetRandomTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayRandomSound();
            SetRandomTimer();
        }
    }

    void SetRandomTimer()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    void PlayRandomSound()
    {
        if (randomClips.Length == 0) return;

        audioSource.clip = randomClips[Random.Range(0, randomClips.Length)];
        audioSource.Play();
    }
}
