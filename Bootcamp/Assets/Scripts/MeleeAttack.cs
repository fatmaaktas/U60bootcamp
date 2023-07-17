using UnityEngine;
using UnityEngine.UI;

public class MeleeAttack : MonoBehaviour
{
    public int damageAmount = 10; 
    public float cooldownDuration = 2f; 
    public float damageDelay = 5f; 
    public float distanceRadius = 5f; 
    public ParticleSystem damageParticle; 
    public Image cooldownImage; 

    private Animator animator; 
    private bool isAttackReady = true; 
    private bool isDamaging = false; 
    private float currentCooldownTime = 0f; 
    private float currentDamageDelay = 3f; 
    public AudioSource audioSource;

    public AudioClip skillSound;

    private void Start()
    {
        animator = GetComponent<Animator>(); 
        cooldownImage.fillAmount = 1f; 
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isAttackReady)
        {
            currentCooldownTime -= Time.deltaTime;

            if (currentCooldownTime <= 0f)
            {
                isAttackReady = true;
                cooldownImage.fillAmount = 1f; 
            }
            else
            {
                float cooldownRatio = currentCooldownTime / cooldownDuration;
                cooldownImage.fillAmount = 1f - cooldownRatio; 
            }
        }

        if (isDamaging)
        {
            currentDamageDelay -= Time.deltaTime;

            if (currentDamageDelay <= 0f)
            {
                isDamaging = false;
                damageParticle.Stop(); 
            }
        }

        if (Input.GetMouseButtonDown(1) && isAttackReady && !isDamaging) 
        {
            PerformDamage();
        }
    }

    private void PerformDamage()
    {
        
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            
            HealthController enemyHealth = closestEnemy.GetComponent<HealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }

            
            DamageParticleActivator particleActivator = closestEnemy.GetComponent<DamageParticleActivator>();
            if (particleActivator != null)
            {
                particleActivator.ActivateDamageParticle();
            }
        }

        
        animator.SetTrigger("MeleeAttack");

        
        isAttackReady = false;
        currentCooldownTime = cooldownDuration;
        cooldownImage.fillAmount = 1f; 

        
        isDamaging = true;
        currentDamageDelay = damageDelay;

        
        damageParticle.Play();
        Invoke("StopDamageParticle", 1f);

        audioSource.PlayOneShot(skillSound, 1f);
    }

    private void StopDamageParticle()
    {
        damageParticle.Stop(); 
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < closestDistance && distance <= distanceRadius)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }
}
