using UnityEngine;
using Unity.AI.Navigation;

public class EnemyNavigation : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform playerTransform;

    public float normalSpeed = 3.5f;
    public float aggroSpeed = 7f;

    public float detectionRange = 10f;
    public LayerMask obstructionMask;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = normalSpeed;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null) {
            agent.SetDestination(playerTransform.position);
        }

        if (CanSeePlayer()) {
            agent.speed = aggroSpeed;
        }
        else {
            agent.speed = normalSpeed;
        }
    }

    bool CanSeePlayer() {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position - transform.position);

        if(distanceToPlayer < detectionRange) {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            if(!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask)) {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
