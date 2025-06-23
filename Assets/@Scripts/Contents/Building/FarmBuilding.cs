using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBuilding : BuildingBase
{ 
    [SerializeField] private Renderer buildingRenderer;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color readyColor = Color.green;
    public GameObject collectIcon;
    
    public override void Init()
    {
        base.Init();
        //collectIcon.SetActive(false);
        if (buildingRenderer != null)
            buildingRenderer.material.color = defaultColor;
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
        
        Debug.Log("농사 완성");
        
        // 아이템 지급
        Debug.Log("[인벤토리] 작물 아이템 지급!");

        // 상태 전이
        CurrentState = BuildingState.ReadyToCollect;
        // collectIcon.SetActive(true);
        buildingRenderer.material.color = readyColor;
    }
    public void Collect()
    {
        if (CurrentState != BuildingState.ReadyToCollect) return;

        Debug.Log("✅ 작물을 수확했습니다.");
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);
            buildingRenderer.material.color = defaultColor;
    }
    public override void OnClick()
    {
        if (CurrentState == BuildingState.ReadyToCollect)
            Collect();
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
