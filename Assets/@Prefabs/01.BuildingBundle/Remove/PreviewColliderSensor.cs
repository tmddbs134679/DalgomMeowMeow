using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger 박스 콜라이더 안에 들어온 오브젝트들을 감지해서 리스트로 저장
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class PreviewColliderSensor : MonoBehaviour
{
    public List<Collider> currentHits = new List<Collider>();
    public string targetLayerName = "Road"; // 검사할 레이어 이름
    private int _targetLayer;

    private void Awake()
    {
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;

        // 문자열로 된 레이어 이름을 정수로 변환
        _targetLayer = LayerMask.NameToLayer(targetLayerName);

        if (_targetLayer == -1)
        {
            Debug.LogError($"[PreviewColliderSensor] Layer \"{targetLayerName}\" is not defined.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other}+하고 충돌");
        if (other.gameObject.layer == _targetLayer && !currentHits.Contains(other))
        {
            currentHits.Add(other);
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (currentHits.Contains(other))
    //     {
    //         currentHits.Remove(other);
    //     }
    // }

    public Collider[] GetCurrentHits()
    {
        return currentHits.ToArray();
    }

    public void ClearHits()
    {
        currentHits.Clear();
    }
}
