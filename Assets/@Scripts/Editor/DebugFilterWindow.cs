using UnityEditor;
using UnityEngine;
using static Define;

public class DebugFilterWindow : EditorWindow
{
    private DebugSettings debugSettings;
    private SerializedObject serializedSettings;
    private SerializedProperty enabledTypesProperty;

    [MenuItem("Tools/Debug Filter Window")]
    public static void ShowWindow()
    {
        GetWindow<DebugFilterWindow>("Debug Filter");
    }

    private void OnEnable()
    {
        // 자동으로 ScriptableObject 참조 (Resources 폴더에 있어야 함)
        debugSettings = Resources.Load<DebugSettings>("DebugSettings");
        if (debugSettings == null)
        {
            Debug.LogError("DebugSettings.asset 없음");
            return;
        }

        serializedSettings = new SerializedObject(debugSettings);
        enabledTypesProperty = serializedSettings.FindProperty("enabledDebugTypes");
    }

    private void OnGUI()
    {
        if (debugSettings == null || serializedSettings == null || enabledTypesProperty == null)
        {
            EditorGUILayout.HelpBox("DebugSettings not loaded.", MessageType.Error);
            return;
        }

        EditorGUILayout.LabelField("Enabled Debug Types", EditorStyles.boldLabel);

        serializedSettings.Update();

        foreach (EDebugType type in System.Enum.GetValues(typeof(EDebugType)))
        {
            bool isEnabled = debugSettings.enabledDebugTypes.Contains(type);
            bool toggle = EditorGUILayout.ToggleLeft(type.ToString(), isEnabled);

            if (toggle && !isEnabled)
                debugSettings.enabledDebugTypes.Add(type);
            else if (!toggle && isEnabled)
                debugSettings.enabledDebugTypes.Remove(type);
        }

        serializedSettings.ApplyModifiedProperties();
        EditorUtility.SetDirty(debugSettings);
    }
}