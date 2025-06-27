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

        StoredCount++; //  생산 누적
        
        Debug.Log($"요리 완성! 누적 수량: {StoredCount}");

        Managers.Food.MakeFood();
        //(Managers.UI.SceneUI as UI_GameScene).ResetCookItem();
        //QuestManager.Instance.OnEvent(QuestConditionType.Collect, TargetType.Soup);
        
        
       // QuestManager.Instance.GiveReward("Soup_10");
        // collectIcon.SetActive(true);
        
        
        if (buildingRenderer != null)
            buildingRenderer.material.color = readyColor;
        
    }
    public override void OnClick()
    {
        if (StoredCount > 0)
        {
            Collect();
        }
    }
    public void Collect()
    {
        if (StoredCount <= 0) return;

        Debug.Log($" {StoredCount}개 요리를 수확했습니다!");

        
        StoredCount = 0;
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);
        if (buildingRenderer != null)
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
