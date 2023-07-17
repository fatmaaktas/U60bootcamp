using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public FireballSkill fireballSkill;
    public Transform handTransform;
    public FireburstSkill fireburstSkill;
    public Transform fireburstSpawnPoint;
    public float attackDuration = 2f;


    

    private bool isFireballReady = true; 
    private float currentCooldownTime = 0f; 

    private bool isFireburstReady = true; 
    private float currentFireburstCooldownTime = 0f; 

    [SerializeField] float maxHealth = 100; 
    [SerializeField] float currentHealth;

    [SerializeField] GameObject circleParticle;
    [SerializeField] float circleParticleActiveTime = 12f;
    [SerializeField] GameObject lightingParticle;
    [SerializeField] float lightingParticleActiveTime = 12f;
    [SerializeField] GameObject lightingSpark;
    [SerializeField] float lightingSparkActiveTime = 12f;
    [SerializeField] GameObject lightingBall;
    [SerializeField] float lightingBallActiveTime = 3f;
    public Slider healthSlider;

    public Image fireballCooldownImage;
    public Image fireburstCooldownImage;
    private bool isTakingDamage = false;
    private Animator _animator;
    
    protected Checkpoint m_CurrentCheckpoint;
    public Vector3 respawnPosition;
    private Portal currentPortal;
    public GameObject pressEButton;


    private void Start()
    {
        currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
        healthSlider.maxValue = maxHealth; 
        healthSlider.value = currentHealth; 
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isFireballReady)
        {
            CastFireball();
            LightingBallActive();
            Invoke("LightingBallDisactive", lightingBallActiveTime);
        }
        else if (Input.GetKeyDown(KeyCode.G) && isFireburstReady)
        {

            ParticleSystemActive();
            Invoke("ParticleSystemDisactive", circleParticleActiveTime);
            Invoke("ParticleSystemDisactive", lightingSparkActiveTime);
            Invoke("ParticleSystemDisactive", lightingParticleActiveTime);
            CastFireburst();

            
        }

        
        if (!isFireballReady)
        {
            currentCooldownTime -= Time.deltaTime;

            if (currentCooldownTime <= 1f)
            {
                isFireballReady = true;
            }

            float cooldownRatio = Mathf.Clamp01(currentCooldownTime / fireballSkill.cooldown);
            fireballCooldownImage.fillAmount = cooldownRatio; 
        }
        else
        {
            fireballCooldownImage.fillAmount = 1f; 
        }

        
        if (!isFireburstReady)
        {
            currentFireburstCooldownTime -= Time.deltaTime;

            if (currentFireburstCooldownTime <= 1f)
            {
                isFireburstReady = true;
            }

            float cooldownRatio = Mathf.Clamp01(currentFireburstCooldownTime / fireburstSkill.cooldown);
            fireburstCooldownImage.fillAmount = cooldownRatio; 
        }
        else
        {
            fireburstCooldownImage.fillAmount = 1f; 
        }
        if (currentPortal != null && Input.GetKeyDown(KeyCode.E))
        {
            currentPortal.DeactivatePortal();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoisonWater"))
        {
            Die();
        }
        if (other.CompareTag("Portal"))
        {
            currentPortal = other.GetComponent<Portal>();
            pressEButton.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            currentPortal = null;
            pressEButton.SetActive(false);
        }
    }

    private void CastFireball()
    {
        
        if (isFireballReady)
        {
            
            Vector3 handPosition = handTransform.position;

            
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(handPosition, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }

            
            if (closestEnemy != null)
            {
                Vector3 targetPosition = closestEnemy.transform.position + Vector3.up * closestEnemy.GetComponent<Collider>().bounds.extents.y;

                
                fireballSkill.Cast(handPosition, targetPosition);

                
                isFireballReady = false;
                currentCooldownTime = fireballSkill.cooldown;

                _animator.SetTrigger("FireballS");
                _animator.SetTrigger("FireballF");
            }
        }
    }

    private void CastFireburst()
    {
        
        if (isFireburstReady && fireburstSkill != null )
        {

            Vector3 spawnPosition = fireburstSpawnPoint.position;

            GameObject fireburst = Instantiate(fireburstSkill.fireburstPrefab, spawnPosition, Quaternion.identity);
            fireburst.transform.rotation = Quaternion.LookRotation(transform.forward);
            
            fireburst.transform.parent = transform;

            fireburstSkill.Cast(spawnPosition);

            
            _animator.SetTrigger("Ulti");
            _animator.SetTrigger("FinishU");

          
            
            isFireburstReady = false;
            currentFireburstCooldownTime = fireburstSkill.cooldown;

         

            
            Destroy(fireburst, 4f);
        }
    }

    void ParticleSystemActive()
    {
        circleParticle.SetActive(true);
        lightingParticle.SetActive(true);
        lightingSpark.SetActive(true);
    }

    void ParticleSystemDisactive()
    {
        circleParticle.SetActive(false);
        lightingParticle.SetActive(false);
        lightingSpark.SetActive(false);
    }

    void LightingBallActive()
    {
        lightingBall.SetActive(true);
    }

    void LightingBallDisactive()
    {
        lightingBall.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!isTakingDamage)
        {
            currentHealth -= damage;
            healthSlider.value = currentHealth;
            StartCoroutine(UpdateDamageOverTime());
            _animator.SetTrigger("Hit");


            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    private IEnumerator UpdateDamageOverTime()
    {
        isTakingDamage = true;
        float timer = 0f;

        while (timer < attackDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        isTakingDamage = false;
    }

    private void Die()
    {
        _animator.SetTrigger("die");
        

        if (m_CurrentCheckpoint != null)
        {
            float respawnDelay = 1.5f; 
            Invoke("RespawnAtCheckpoint", respawnDelay);
            currentHealth = maxHealth;
            healthSlider.value += currentHealth;
            
            
        }
        else
        {
            Invoke("RespawnAtPosition", 1.5f); 
            currentHealth = maxHealth;
            healthSlider.value += currentHealth;

        }

    }

    private void RespawnAtCheckpoint()
    {
        
        transform.position = m_CurrentCheckpoint.transform.position;
        transform.rotation = m_CurrentCheckpoint.transform.rotation;
        _animator.SetTrigger("dieRec");
        
    }

    private void RespawnAtPosition()
    {
        transform.position = respawnPosition;
        _animator.SetTrigger("dieRec");

    }


    public void SetCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint != null)
            m_CurrentCheckpoint = checkpoint;
            
    }




}
