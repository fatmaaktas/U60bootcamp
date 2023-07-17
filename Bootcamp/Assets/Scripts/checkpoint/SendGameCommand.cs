using UnityEngine;

[SelectionBase]
public class SendGameCommand : MonoBehaviour
{
    public GameCommandType interactionType;
    public GameCommandReceiver interactiveObject;
    public bool oneShot = false;
    public float coolDown = 1f;
    public AudioSource onSendAudio;
    public float audioDelay;

    private float lastSendTime;
    private bool isTriggered = false;

    public float Temperature
    {
        get
        {
            return 1f - Mathf.Clamp01(Time.time - lastSendTime);
        }
    }

    [ContextMenu("Send Interaction")]
    public void Send()
    {
        if (oneShot && isTriggered)
            return;

        if (Time.time - lastSendTime < coolDown)
            return;

        isTriggered = true;
        lastSendTime = Time.time;

        interactiveObject.Receive(interactionType);

        if (onSendAudio)
            onSendAudio.PlayDelayed(audioDelay);
    }

    protected virtual void Reset()
    {
        interactiveObject = GetComponent<GameCommandReceiver>();
    }
}

