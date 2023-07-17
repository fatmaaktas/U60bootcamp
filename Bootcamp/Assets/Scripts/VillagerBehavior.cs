using UnityEngine;
using UnityEngine.AI;

public class VillagerBehavior : MonoBehaviour
{
    public float idleDuration = 3f; 
    public float walkRadius = 10f; 

    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 startPosition;
    private Vector3 randomDestination;
    private float idleTimer;
    private bool isIdle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        startPosition = transform.position;
        SetRandomDestination();
    }

    private void Update()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                SetRandomDestination();
                idleTimer = 0f;
                isIdle = false;
                animator.SetBool("IsWalking", true);
            }
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isIdle = true;
                animator.SetBool("IsWalking", false);
            }
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += startPosition;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, walkRadius, -1);
        randomDestination = navMeshHit.position;
        agent.SetDestination(randomDestination);
    }
}

