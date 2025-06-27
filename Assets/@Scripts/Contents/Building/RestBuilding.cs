using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestBuilding : BuildingBase
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
        Debug.Log("휴식 완료");


        // 상태 전이
        CurrentState = BuildingState.ReadyToCollect;
        // collectIcon.SetActive(true);
       // buildingRenderer.material.color = readyColor;
    }
    public void Collect()
    {
        if (CurrentState != BuildingState.ReadyToCollect) return;

        //스테미나 회복
        Debug.Log("스테미나 회복");
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);
        buildingRenderer.material.color = defaultColor;
    }
    public override void OnClick()
    {
        if (CurrentState == BuildingState.ReadyToCollect)
            Collect();
    }

}
