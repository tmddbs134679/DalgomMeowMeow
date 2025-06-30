using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;

    public GameObject collectIcon;
    
    public override bool Init()
    {
        base.Init();
        //collectIcon.SetActive(false);
        return true;
    }
    public override void Produce()
    {
        Debug.Log("낚시 완료");
        
        StoredCount++; //  생산 누적
        
        Debug.Log($"낚시 완료! 누적 수량: {StoredCount}");

        // collectIcon.SetActive(true);

    }
    public void Collect()
    {
        if (StoredCount <= 0) return;

        Debug.Log($" {StoredCount}마리 물고기를 획득");

        StoredCount = 0;
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);

    }
    public override void OnClick()
    {
        if (StoredCount > 0)
        {
            Collect();
        }
    }

}
