using UnityEngine;
using System.Collections;

public class DamageParticleActivator : MonoBehaviour
{
    public ParticleSystem damageParticle; 
    public float particleDuration = 1f; 

    private void Start()
    {
        damageParticle.Stop(); 
    }

    public void ActivateDamageParticle()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Vector3 colliderCenter = collider.bounds.center;
            damageParticle.transform.position = colliderCenter;
        }

        damageParticle.gameObject.SetActive(true); 

        StartCoroutine(DeactivateParticleAfterDuration());
    }

    private IEnumerator DeactivateParticleAfterDuration()
    {
        yield return new WaitForSeconds(particleDuration);

        damageParticle.gameObject.SetActive(false); 
    }
}