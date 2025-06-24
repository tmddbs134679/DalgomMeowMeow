using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color readyColor = Color.green;
    public GameObject collectIcon;
    

    public override void Init()
    {
        base.Init();
        // collectIcon.SetActive(false);
        if (buildingRenderer != null)
            buildingRenderer.material.color = defaultColor;
    }

    public override void OnClick()
    {
        if (CurrentState == BuildingState.ReadyToCollect)
        {
            Collect();
        }
        else
        {
            UI_BuildingInfo ui = FindObjectOfType<UI_BuildingInfo>();
            if (ui != null)
                ui.Open(this);
        }
    }

    public override void Produce()
    {
        //재료 재고 확인  
        if (!HasRequiredMaterials())
        {
            Debug.LogWarning("재료 부족 - 요리 실패");
            return;
        }
        //재료소진
        ConsumeMaterials();
        
        Debug.Log("요리 완성");
        
        // 아이템 지급
        Debug.Log("[인벤토리] 스프 아이템 지급!");

        // 상태 전이
        CurrentState = BuildingState.ReadyToCollect;
        // collectIcon.SetActive(true);
        
            /*buildingRenderer.material.color = readyColor;*/
        
    }
    
    public void Collect()
    {
        if (CurrentState != BuildingState.ReadyToCollect) return;

        Debug.Log(" 요리를 수확했습니다.");
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);
            buildingRenderer.material.color = defaultColor;
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
