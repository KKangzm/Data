using UnityEngine;
using UnityEngine.AI;

public class MutantAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float moveSpeed = 3.5f;
    
    private NavMeshAgent agent;
    private Animator animator;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            StartChasing();
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);

            if (distanceToPlayer < attackRange)
            {
                AttackPlayer();
            }
        }
    }

    void StartChasing()
    {
        if (!isChasing)
        {
            Debug.Log("Mutant has spotted the player!");
            isChasing = true;
            animator.SetBool("isRunning", true);
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Mutant is attacking the player!");
        animator.SetTrigger("attack");
        // 触发伤害
    }
}
