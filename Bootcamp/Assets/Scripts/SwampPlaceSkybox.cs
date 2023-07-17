using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampPlaceSkybox : MonoBehaviour
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
            
            isNight = true;
            RenderSettings.skybox = nightSkybox;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == triggerCollider)
        {
            
            isNight = false;
            RenderSettings.skybox = originalSkybox;
        }
    }
}
