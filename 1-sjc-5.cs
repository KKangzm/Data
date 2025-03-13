// InfectedAI.cs
public class InfectedAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }
    public AIState currentState;

    private NavMeshAgent agent;
    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine(){
        while(true){
            if(currentState == AIState.Patrol){
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                currentPatrolIndex = (currentPatrolIndex +1)% patrolPoints.Length;
                yield return new WaitForSeconds(10f);
            }
            yield return null;
        }
    }

    public void DetectPlayer(Transform player){
        currentState = AIState.Chase;
        agent.SetDestination(player.position);
    }
}