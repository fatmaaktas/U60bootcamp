using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class HealthController : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    public int currentHealth;
    [SerializeField] Slider healthBar;

    private Animator animator;
    private EnemyController enemyController;
    

    [SerializeField] float destroyDelay = 2f;
    [SerializeField] float delayTime = 1.0f; 


    public event System.Action OnDeath; 

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        
    }

    private void Update()
    {
        if (healthBar.value > 0)
        healthBar.value = currentHealth;

        else if (healthBar.value == 0)
        {
            healthBar.gameObject.SetActive(false);
        }
        
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
              
                Die();
            }
            else
            {
                animator.SetTrigger("damage");
            }
        }
    }

    private void Die()
    {

        animator.SetTrigger("Die");

        enemyController.SetPlayerDead(true);

        OnDeath?.Invoke(); 

        
        Invoke("DestroyEnemy", destroyDelay);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void TakeDamageWithDelay(int damage)
    {
        StartCoroutine(DelayedDamage(damage));
    }

    private IEnumerator DelayedDamage(int damage)
    {
        yield return new WaitForSeconds(delayTime); 
        TakeDamage(damage); 
    }

}

