using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArrayMapPos))]
public class ArrayMapPosEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ArrayMapPos map = (ArrayMapPos)target;

        if (GUILayout.Button("맵 초기화하기"))
        {
            map.InitializeMap(); // 원하는 너비/높이로 설정
        }
    }
}
