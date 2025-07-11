using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBattleContext
{
    public static int PendingUnlockForestId = -1;
    public static bool IsVictory = false;
}
public class ForestRegion : BaseObject
{
    public int Id;
    public bool IsUnlocked { get; private set; }

    [SerializeField] private GameObject lockOverlay; // 잠김 표시용 UI/이펙트

    private void Start()
    {
        LoadUnlockState();
        
        //UpdateVisual();
    }

    // 전투에서 승리하면 외부에서 이 함수 호출
    public void Unlock()
    {
        if (IsUnlocked) return;

        IsUnlocked = true;
        SaveUnlockState();
        //UpdateVisual();
        BuildingPlacer.Instance.DeleteStage(gameObject);
    }

    private void UpdateVisual()
    {
        lockOverlay.SetActive(!IsUnlocked);
        gameObject.SetActive(!IsUnlocked);
        gameObject.SetActive(IsUnlocked);
        
        // 애니메이션 or 이펙트 재생도 여기에
    }

    private void SaveUnlockState()
    {
        ForestSaveData data = SaveForestTest.Load();
        if (!data.unlockedRegionIds.Contains(Id))
        {
            data.unlockedRegionIds.Add(Id);
            SaveForestTest.Save(data);
        }
    }

    private void LoadUnlockState()
    {
        ForestSaveData data = SaveForestTest.Load();
        IsUnlocked = data.unlockedRegionIds.Contains(Id);
    }


    public override void OnClick()
    {
        if (IsUnlocked)
        {
            Managers.Debug.Log($"{Id}해제된 지역",Define.EDebugType.Building);
            // Debug.Log($"{Id}해제된 지역");
            return;
        }
        if (!IsUnlocked)
        {
            ForestBattleContext.PendingUnlockForestId = Id;
            Managers.Debug.Log($"[TEST] Forest {Id} 클릭됨 → BattleScene 이동",Define.EDebugType.Building);
            // Debug.Log($"[TEST] Forest {Id} 클릭됨 → BattleScene 이동");
            UI_ForestPopup popup = Managers.UI.ShowPopupUI<UI_ForestPopup>();
            //전투 씬 이동
        }
    }
}

