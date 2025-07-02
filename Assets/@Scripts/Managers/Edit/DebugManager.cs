using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class DebugManager
{
    public DebugSettings debugSettings;

    public void Log(string message, EDebugType type)
    {
        if (IsEnabled(type))
        {
            Debug.Log($"[{type}] {message}");
        }
    }

    public void LogWarning(string message, EDebugType type)
    {
        if (IsEnabled(type))
        {
            Debug.LogWarning($"[{type}] {message}");
        }
    }

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