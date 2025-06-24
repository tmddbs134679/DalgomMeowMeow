using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingPlacer))]
public class BuildingPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildingPlacer placer = (BuildingPlacer)target;

        if (GUILayout.Button("📌 BuildSO 폴더에서 자동 등록"))
        {
            string folderPath = "Assets/Scripts/BuildMap/ScriptableOBJ/BuildSO";
            string[] guids = AssetDatabase.FindAssets("t:TestBaseBuilding", new[] { folderPath });

           // placer.buildingSOList = new List<TestBaseBuilding>();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                TestBaseBuilding so = AssetDatabase.LoadAssetAtPath<TestBaseBuilding>(assetPath);
            //    if (so != null)
                //    placer.buildingSOList.Add(so);
            }

          //  Debug.Log($"✅ {placer.buildingSOList.Count}개의 SO를 자동 등록했습니다.");
            EditorUtility.SetDirty(placer);
        }
    }
}
