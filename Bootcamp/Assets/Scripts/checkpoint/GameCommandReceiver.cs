using System.Collections.Generic;
using UnityEngine;

public class GameCommandReceiver : MonoBehaviour
{
    private Dictionary<GameCommandType, List<System.Action>> handlers = new Dictionary<GameCommandType, List<System.Action>>();

    public static GameCommandReceiver Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Receive(GameCommandType commandType)
    {
        if (handlers.TryGetValue(commandType, out List<System.Action> callbacks))
        {
            foreach (System.Action callback in callbacks)
            {
                callback.Invoke();
            }
        }
    }

    public void Register(GameCommandType commandType, System.Action callback)
    {
        if (!handlers.TryGetValue(commandType, out List<System.Action> callbacks))
        {
            callbacks = new List<System.Action>();
            handlers.Add(commandType, callbacks);
        }
        callbacks.Add(callback);
    }

    public void Remove(GameCommandType commandType, System.Action callback)
    {
        if (handlers.TryGetValue(commandType, out List<System.Action> callbacks))
        {
            callbacks.Remove(callback);
        }
    }
}

