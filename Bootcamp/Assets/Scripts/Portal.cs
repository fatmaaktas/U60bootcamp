using UnityEngine;

public class Portal : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public GameObject portalTick; 

    private bool isActive = true;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void DeactivatePortal()
    {
        if (isActive)
        {
            isActive = false;
            particleEffect.Stop(); 
            portalTick.SetActive(true); 
            gameManager.PortalDeactivated(this); 
        }
    }
}
