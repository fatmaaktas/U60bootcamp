using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerHamam : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float attackDistance = 2f;
    [SerializeField] float attackInterval = 3f;

    private Transform player;
    private Rigidbody rb;
    private Animator animator;
    private HealthController healthController;

    [SerializeField] int attackDamage = 10;

    private bool isAttacking = false;
    private bool isPlayerDead = false;
    private float attackTimer = 0f;

    private float destroyTimer = 3f;
    private bool shouldDestroy = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
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

        
        if ((Vector3.Distance(transform.position, player.position) <= detectionRadius) && !isPlayerDead)
        {
            
            Vector3 targetPosition = player.position - player.forward * attackDistance; // Oyuncunun arkasýný hedef olarak belirle
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            // Oyuncuya doðru hareket et ve dön
            if (!isAttacking && !isPlayerDead && healthController.currentHealth > 0)
            {
                rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(-direction), rotationSpeed * Time.deltaTime));
            }

            
            bool isWalking = (isPlayerDead && !isAttacking && direction.magnitude > 5f);
            animator.SetBool("IsWalking", true);

           
            if (!isAttacking && Vector3.Distance(transform.position, player.position) <= attackDistance)
            {
                
                if (attackTimer <= 0f)
                {
                    isAttacking = true;
                    animator.SetBool("IsAttacking", true);
                    AttackPlayer();
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

        if (healthController.currentHealth <= 0)
        {
           
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0f)
            {
                shouldDestroy = true;
            }
        }

        if (shouldDestroy)
        {
            Destroy(gameObject);
            return; 
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

        if (isPlayerDead)
        {
            rb.velocity = Vector3.zero; 
        }
    }
}
