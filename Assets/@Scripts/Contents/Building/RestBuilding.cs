using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;

    public GameObject collectIcon;
    public override void Init()
    {
        base.Init();
        // collectIcon.SetActive(false);

    }
    public override void Produce()
    {
        Debug.Log("휴식 완료");


        // 상태 전이
        CurrentState = BuildingState.ReadyToCollect;
        // collectIcon.SetActive(true);

    }
    public void Collect()
    {
        if (CurrentState != BuildingState.ReadyToCollect) return;

        //스테미나 회복
        Debug.Log("스테미나 회복");
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);

    }
    public override void OnClick()
    {
        if (CurrentState == BuildingState.ReadyToCollect)
            Collect();
    }

}
