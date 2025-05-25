using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    [Tooltip("Arrastra aquí tu componente Text (UI) dentro de un Canvas.")]
    [SerializeField] private Text consoleText;

    [Tooltip("Número máximo de líneas a mostrar.")]
    [SerializeField] private int maxLines = 50;

    private readonly Queue<string> logLines = new Queue<string>();

    void Awake()
    {
        if (consoleText == null)
        {
            Debug.LogError("DebugConsole: debes asignar un componente Text en el Inspector.");
            enabled = false;
            return;
        }
        
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string prefix = type == LogType.Error || type == LogType.Exception ? "<color=red>[ERROR]</color> "
            : type == LogType.Warning ? "<color=yellow>[WARN]</color> "
            : "[LOG] ";
        
        logLines.Enqueue(prefix + logString);
        
        while (logLines.Count > maxLines)
            logLines.Dequeue();
        
        consoleText.text = string.Join("\n", logLines.ToArray());
    }
    
    public void ClearConsole()
    {
        logLines.Clear();
        consoleText.text = "";
    }
}