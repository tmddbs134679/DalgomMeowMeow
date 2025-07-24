#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[ExecuteInEditMode]
public class MapRegionExtractor : MonoBehaviour
{
    [Header("📦 추출 영역")]
    public Vector3 regionCenter = Vector3.zero;
    public Vector3 regionSize = new Vector3(10, 10, 10);

    [Header("💾 저장 경로")]
    public string prefabSavePath = "Assets/ExtractedRegions/Region.prefab";

    [ContextMenu("🔧 Transform 기준 추출 → 프리팹 저장")]
    public void ExtractRegionByTransform()
    {
        Transform[] all = GameObject.FindObjectsOfType<Transform>(true);
        HashSet<GameObject> included = new HashSet<GameObject>();

        foreach (Transform t in all)
        {
            if (t == this.transform || t.parent == null) continue;

            Vector3 pos = t.position;
            if (IsInsideBox(pos, regionCenter, regionSize))
            {
                GameObject go = t.gameObject;
                if (!EditorUtility.IsPersistent(go) && !included.Contains(go))
                {
                    included.Add(go);
                }
            }
        }

        if (included.Count == 0)
        {
            Debug.LogWarning("⚠️ 해당 영역 안에 감지된 오브젝트가 없습니다.");
            return;
        }

        GameObject root = new GameObject("ExtractedRegion");

        foreach (GameObject go in included)
        {
            GameObject clone = Instantiate(go, go.transform.position, go.transform.rotation, root.transform);
            clone.transform.localScale = go.transform.localScale;
        }

        // 저장 경로 폴더 없으면 생성
        string folder = Path.GetDirectoryName(prefabSavePath);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        PrefabUtility.SaveAsPrefabAsset(root, prefabSavePath);
        Debug.Log($"✅ 프리팹 저장 완료: {prefabSavePath}");

        DestroyImmediate(root);
    }

    private bool IsInsideBox(Vector3 pos, Vector3 center, Vector3 size)
    {
        Vector3 min = center - size * 0.5f;
        Vector3 max = center + size * 0.5f;
        return pos.x >= min.x && pos.x <= max.x &&
               pos.y >= min.y && pos.y <= max.y &&
               pos.z >= min.z && pos.z <= max.z;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.2f, 0.2f);
        Gizmos.DrawCube(regionCenter, regionSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(regionCenter, regionSize);
    }
}
#endif
