using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float detectionRadius = 10f;
    public float attackDistance = 2f;
    public float attackInterval = 3f;

    private Transform player;
    Rigidbody rb;
    private Animator animator;

    public int attackDamage = 10; 

    private bool isAttacking = false;
    private bool isPlayerDead = false; 
    private float attackTimer = 0f;



    private void Awake()
    {

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


    }

    private void Update()
    {
        if (isPlayerDead)
        {
            
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", false);
            return;
        }

        
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            
            Vector3 targetPosition = player.position - player.forward * attackDistance; 
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            

            
            if (!isAttacking)
            {
                rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(-direction), rotationSpeed * Time.deltaTime));
            }

            
            bool isWalking = (!isAttacking && direction.magnitude > 5f);
            animator.SetBool("IsWalking",true);

            if (!isAttacking && Vector3.Distance(transform.position, player.position) <= attackDistance)
            {
                
                if (attackTimer <= 0f)
                {
                    isAttacking = true;
                    animator.SetBool("IsAttacking", true);
                    AttackPlayer();

                    
                    attackTimer = attackInterval;
                }
                else
                {
                    
                    attackTimer -= Time.deltaTime;
                }
            }
        }
        else
        {
            
            animator.SetBool("IsWalking", false);
        }

        
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1f)
            {
                isAttacking = false;
                animator.SetBool("IsAttacking", false);
            }
        }
    }

    private void AttackPlayer()
    {
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.TakeDamage(attackDamage);
            }
        }
    }

    public void SetPlayerDead(bool isDead)
    {
        isPlayerDead = isDead;
    }



}