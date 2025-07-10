using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBuilding : BuildingBase
{ 
    [SerializeField] private Renderer buildingRenderer;
    public GameObject collectIcon;
    public event Action IsHarvest;

    public override bool Init()
    {
        base.Init();
        //collectIcon.SetActive(false);
        return true;
    }
    public override void Produce()
    {
        
        //재료 재고 확인  
        if (!HasRequiredMaterials())
        {
            Debug.LogWarning("재료 부족 - 농사 실패");
            return;
        }
        //재료소진
        ConsumeMaterials();
        
        
        StoredCount++; //  생산 누적
        
        IsHarvest?.Invoke();//수확 가능
        

        CurrentState = BuildingState.ReadyToCollect;
        // collectIcon.SetActive(true);
        QuestManager.Instance.OnEvent(QuestConditionType.Collect, TargetType.Farm);

    }
    public void Collect()
    {
        if (StoredCount <= 0) return;

        Debug.Log($" {StoredCount}개 야채를 수확했습니다!");

        StoredCount = 0;
        CurrentState = BuildingState.Idle;
        // collectIcon.SetActive(false);

    }
    public override void OnClick()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        UI_BuildContent popup = Managers.UI.ShowPopupUI<UI_BuildContent>();
        popup.SetTarget(gameObject);
    }
    
    private bool HasRequiredMaterials()
    {
        //인벤토리에 재료 확인
        return true;
    }
    
    private void ConsumeMaterials()
    {
        //재료 소비
    }
}
