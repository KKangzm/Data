using UnityEngine;
using UnityEngine.AI;

public class GhostPirateAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint = 0;
    private NavMeshAgent agent;
    public float detectionRange = 10f;
    public Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = patrolPoints[currentPoint].position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            agent.destination = player.position;
        }
        else if (agent.remainingDistance < 1f)
        {
            NextPatrolPoint();
        }
    }

    void NextPatrolPoint()
    {
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
        agent.destination = patrolPoints[currentPoint].position;
    }
}
