using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestRegion : BaseObject
{
    public int Id;
    public bool IsUnlocked { get; private set; }

    [SerializeField] private GameObject lockOverlay; // 잠김 표시용 UI/이펙트

    private void Start()
    {
        LoadUnlockState();
        UpdateVisual();
    }

    // 전투에서 승리하면 외부에서 이 함수 호출
    public void Unlock()
    {
        if (IsUnlocked) return;

        IsUnlocked = true;
        SaveUnlockState();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        lockOverlay.SetActive(!IsUnlocked);
        // 애니메이션 or 이펙트 재생도 여기에
    }

    private void SaveUnlockState()
    {
        // ForestSaveData data = 
        // if (!data.unlockedRegionIds.Contains(Id))
        // {
        //     data.unlockedRegionIds.Add(Id);
        //     SaveManager.Save(data);
        // }
    }

    private void LoadUnlockState()
    {
        // ForestSaveData data = GameManager
        // IsUnlocked = data.unlockedRegionIds.Contains(Id);
    }

    public override void OnClick()
    {
        
    }
}

