using UnityEditor;
using UnityEngine;

/// <summary>
/// ArrayBuildPos 에디터
/// </summary>
[CustomEditor(typeof(ArrayBuildPos))]
public class ArrayBuildPosEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ArrayBuildPos Build = (ArrayBuildPos)target;

        if (GUILayout.Button("건물 초기화하기"))
        {
            Build.InitializeBuild();
        }
    }
}
