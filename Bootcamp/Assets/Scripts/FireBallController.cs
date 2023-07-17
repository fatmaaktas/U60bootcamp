using UnityEngine;

public class FireballController : MonoBehaviour
{
    private FireballSkill fireballSkill;
    private int damage;
    private float cooldown;
    private float lifetime = 2f; 

    private float elapsedTime = 0f; 

    public void SetFireballSkill(FireballSkill skill)
    {
        fireballSkill = skill;
        damage = fireballSkill.damage;
        cooldown = fireballSkill.cooldown;
    }

    private void Update()
    {
        
        elapsedTime += Time.deltaTime;

        
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        GameObject target = collision.gameObject;

        
        if (target.CompareTag("Player"))
            return;

        if (target != null)
        {
            HealthController healthController = target.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.TakeDamage(damage);
            }
        }

        
        Destroy(gameObject);
    }
}
