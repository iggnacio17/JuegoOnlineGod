using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement_script : MonoBehaviour
{
    public float walkRadius = 20f;
    public float waitTime = 6f;
    private Animator anim_npc;

    private NavMeshAgent agent;
    private float waitTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToRandomPoint();
        anim_npc = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                GoToRandomPoint();
                waitTimer = 0f;
            }
        }

        float speed = agent.velocity.magnitude;
        anim_npc.SetBool("isWalking", speed > 0.1f);
    }

    void GoToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}