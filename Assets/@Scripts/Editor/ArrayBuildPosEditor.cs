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

        if (GUILayout.Button("캐싱 된 건물 초기화하기"))
        {
            Build.InitializeBuild();
        }

        if (GUILayout.Button("데이터 저장"))
        {
            Build.SaveMapData();
        }

        if (GUILayout.Button("데이터 불러오기"))
        {
            Build.EditorLoadMapData();
        }

        if (GUILayout.Button("기초 맵 불러오기"))
        {
            Build.LoadProtoTypeMapData();
        }
    }
}
