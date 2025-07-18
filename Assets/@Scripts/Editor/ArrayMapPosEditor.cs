using UnityEditor;
using UnityEngine;

/// <summary>
/// ArrayMapPos 에디터
/// </summary>
[CustomEditor(typeof(ArrayMapPos))]
public class ArrayMapPosEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ArrayMapPos map = (ArrayMapPos)target;

        if (GUILayout.Button("캐싱 된 맵 초기화하기"))
        {
            map.InitializeMap(); // 원하는 너비/높이로 설정
        }

        if (GUILayout.Button("데이터 저장"))
        {
            map.SaveMapTileData();
        }

        if (GUILayout.Button("데이터 불러오기"))
        {
            map.EditorLoadMapTileData();
        }
        
        if (GUILayout.Button("기초데이터 불러오기"))
        {
            map.LoadProtoTypeMapTileData();
        }
    }
}
