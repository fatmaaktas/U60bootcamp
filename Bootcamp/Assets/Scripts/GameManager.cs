using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Portal redPortal;
    public Portal yellowPortal;
    public Portal bluePortal;
    public Portal greenPortal;

    private int deactivatedPortals = 0;
    private int totalPortals = 4;

    private void Update()
    {
        if (deactivatedPortals == totalPortals)
        {
            
            SceneManager.LoadScene("WinGame"); 
        }
    }

    public void PortalDeactivated(Portal portal)
    {
        deactivatedPortals++;

        if (deactivatedPortals == totalPortals)
        {
            
            SceneManager.LoadScene("WinGame"); 
        }
    }
}
