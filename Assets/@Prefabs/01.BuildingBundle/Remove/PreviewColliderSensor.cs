using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger 박스 콜라이더 안에 들어온 오브젝트들을 감지해서 리스트로 저장
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class PreviewColliderSensor : MonoBehaviour
{
    public List<Collider> currentHits = new List<Collider>();
    public string targetTag = "Road"; // 태그 기반 필터링 (선택사항)

    private void Awake()
    {
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true; // 꼭 Trigger여야 감지됨
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !currentHits.Contains(other))
        {
            currentHits.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentHits.Contains(other))
        {
            currentHits.Remove(other);
        }
    }

    public Collider[] GetCurrentHits()
    {
        return currentHits.ToArray();
    }

    public void ClearHits()
    {
        currentHits.Clear();
    }
}
