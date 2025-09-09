using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Get current velocity speed
        float speed = agent.velocity.magnitude;

        // Send it to the Animator
        anim.SetFloat("Speed", speed);
    }
}
