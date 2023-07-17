using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SendGameCommand), true)]
public class SendGameCommandEditor : Editor
{
    private void OnSceneGUI()
    {
        var sendGameCommand = (SendGameCommand)target;
        if (sendGameCommand.interactiveObject != null)
        {
            DrawInteraction(sendGameCommand);
        }
    }

    private void DrawInteraction(SendGameCommand sendGameCommand)
    {
        var start = sendGameCommand.transform.position;
        var end = sendGameCommand.interactiveObject.transform.position;
        var dir = (end - start).normalized;

        if (Application.isPlaying)
        {
            Handles.color = Color.Lerp(Color.white, Color.green, sendGameCommand.Temperature);
        }
        else
        {
            Handles.color = new Color(1f, 1f, 1f, 0.25f);
        }

        var steps = Mathf.FloorToInt((end - start).magnitude);
        for (var i = 0; i < steps; i++)
        {
            Handles.ArrowHandleCap(0, start + dir * i, Quaternion.LookRotation(dir), 1f, EventType.Repaint);
        }
    }
}