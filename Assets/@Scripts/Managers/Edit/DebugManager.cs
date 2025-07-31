using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class DebugManager
{
    public DebugSettings debugSettings;

    [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public void Log(string message, EDebugType type)
    {
        if (IsEnabled(type))
        {
            Debug.Log($"[{type}] {message}");
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public void LogWarning(string message, EDebugType type)
    {
        if (IsEnabled(type))
        {
            Debug.LogWarning($"[{type}] {message}");
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public void LogError(string message, EDebugType type)
    {
        if (IsEnabled(type))
        {
            Debug.LogError($"[{type}] {message}");
        }
    }

    public bool IsEnabled(EDebugType type)
    {
        return debugSettings != null && debugSettings.enabledDebugTypes.Contains(type);
    }
}