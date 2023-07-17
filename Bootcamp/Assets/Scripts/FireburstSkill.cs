using UnityEngine;

[CreateAssetMenu(fileName = "New Fireburst Skill", menuName = "Skills/Fireburst")]
public class FireburstSkill : ScriptableObject
{
    public string skillName = "Fireburst";
    public int damage = 25;
    public float cooldown = 10f;
    public GameObject fireburstPrefab;
    public AudioClip castSound;
    public float fireburstRadius = 5f;
 


    public void Cast(Vector3 position)
    {
        // Fireburst efektini yarat ve etkileþime giren düþmanlara hasar ver
        Collider[] colliders = Physics.OverlapSphere(position, fireburstRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                HealthController healthController = collider.GetComponent<HealthController>();
                if (healthController != null)
                {
                    healthController.TakeDamageWithDelay(damage); 
                }
            }
        }
    }


}