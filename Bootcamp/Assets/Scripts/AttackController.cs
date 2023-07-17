using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Animator _animator;
    private bool _isAttacking = false;

    [SerializeField] GameObject gojoRedHollow; 
    [SerializeField] float gojoRedHollowActiveTime = 13f; 
    [SerializeField] GameObject cirleParticle;
    [SerializeField] float circleParticleActiveTime=13f;
    [SerializeField] GameObject lightingParticle;
    [SerializeField] float ligthingParticleActiveTime = 13f;
    [SerializeField] GameObject lightingSpark;
    [SerializeField] float ligtingSparkActiveTime = 13f;
    [SerializeField] GameObject portal;
    [SerializeField] float portalActiveTime = 13f;



    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        
        
        if (Input.GetKeyDown(KeyCode.C) && !_isAttacking)
        {
            StartAttack();

            _animator.SetTrigger("idle");
        }
    }

    private void StartAttack()
    {
        
        _animator.SetTrigger("Attack");

        
        _isAttacking = true;
        gojoRedHollow.SetActive(true);
        cirleParticle.SetActive(true);
        lightingParticle.SetActive(true);
        lightingSpark.SetActive(true);

        
        Invoke("DisableGojoRedHollow", gojoRedHollowActiveTime);
        Invoke("DisablecirleParticle", circleParticleActiveTime);
        Invoke("DisablecirleParticle", ligtingSparkActiveTime);
        Invoke("DisablecirleParticle", ligthingParticleActiveTime);
        Invoke("DisablePortal", portalActiveTime);
    }

    private void DisableGojoRedHollow()
    {
        gojoRedHollow.SetActive(false);
        cirleParticle.SetActive(false);
        lightingSpark.SetActive(false);
        lightingParticle.SetActive(false);

        
        _isAttacking = false;
    }
    private void DisablePortal()
    {
        portal.SetActive(false);

        
        _isAttacking = false;
    }
}
