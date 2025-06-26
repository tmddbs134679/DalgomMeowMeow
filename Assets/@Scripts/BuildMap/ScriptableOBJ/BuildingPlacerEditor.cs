using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BuildingPlacer 에디터
/// </summary>
[CustomEditor(typeof(BuildingPlacer))]
public class BuildingPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildingPlacer placer = (BuildingPlacer)target;

        if (GUILayout.Button(" BuildSO 폴더에서 자동 등록"))
        {
            string folderPath = "Assets/@Scripts/BuildMap/ScriptableOBJ/BuildSO";
            string[] guids = AssetDatabase.FindAssets("t:TestBaseBuilding", new[] { folderPath });

            List<TestBaseBuilding> soList = new List<TestBaseBuilding>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TestBaseBuilding so = AssetDatabase.LoadAssetAtPath<TestBaseBuilding>(path);
                if (so != null)
                    soList.Add(so);
            }

            //  배열로 변환해서 넣기
            placer.buildingSO = soList.ToArray();

            Debug.Log($" {soList.Count}개의 SO가 자동 등록되었습니다.");
            EditorUtility.SetDirty(placer);
        }
    }
}
