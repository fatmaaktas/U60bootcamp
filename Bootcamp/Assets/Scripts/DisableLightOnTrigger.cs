using UnityEngine;

public class DisableLightChangeSkybox : MonoBehaviour
{
    public Light directionalLight;
    public Collider triggerCollider;
    public Material daySkybox;
    public Material nightSkybox;

    private bool isNight = false;
    private Material originalSkybox;

    private void Start()
    {
        originalSkybox = RenderSettings.skybox;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == triggerCollider)
        {
            directionalLight.enabled = false;
            isNight = true;
            RenderSettings.skybox = nightSkybox;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == triggerCollider)
        {
            directionalLight.enabled = true;
            isNight = false;
            RenderSettings.skybox = originalSkybox;
        }
    }
}


