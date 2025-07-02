using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;



[CreateAssetMenu(menuName = "Debug/DebugFilterSettings")]
public class DebugSettings : ScriptableObject
{
    public List<EDebugType> enabledDebugTypes = new List<EDebugType>();
}