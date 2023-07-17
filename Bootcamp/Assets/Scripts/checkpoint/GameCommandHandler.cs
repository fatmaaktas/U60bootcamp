using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCommandHandler : MonoBehaviour
{
    public GameCommandType interactionType;
    public bool isOneShot = false;
    public float coolDown = 0;
    public float startDelay = 0;

    protected bool isTriggered = false;
    private float startTime = 0;

    public abstract void PerformInteraction();

    public virtual void OnInteraction()
    {
        if (isOneShot && isTriggered) return;
        isTriggered = true;
        if (coolDown > 0)
        {
            if (Time.time > startTime + coolDown)
            {
                startTime = Time.time + startDelay;
                ExecuteInteraction();
            }
        }
        else
            ExecuteInteraction();
    }

    private void ExecuteInteraction()
    {
        if (startDelay > 0)
            StartCoroutine(PerformInteractionWithDelay(startDelay));
        else
            PerformInteraction();
    }

    private IEnumerator PerformInteractionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PerformInteraction();
    }

    protected virtual void Awake()
    {
        GameCommandReceiver.Instance.Register(interactionType, new System.Action(OnInteraction));
    }
}
