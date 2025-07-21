using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FarmBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;
    public GameObject collectIcon;
    public event Action IsHarvest;
    
    public Define.ECropType CropType;
    public Define.ECropType GetCropType() => CropType;

    private void Awake()
    {
        _textAnim = Managers.UI.ShowPopupUI<UI_TextAnimation>();

    }
    protected override void Start()
    {
        base.Start();

        _textAnim.gameObject.SetActive(false);
        _textAnim.SetInfo(Define.EBuildingType.CabbageFarm, transform.position);

    }

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


        CurrentState = BuildingState.ReadyToCollect;
        // collectIcon.SetActive(true);
        QuestManager.Instance.UpdateQuestProgress(Define.EQuestConditionType.Collect, Define.ETargetType.Farm);

    }

    public override void ConnectToAnimal(AICharacter animal)
    {
        base.ConnectToAnimal(animal);

        _textAnim.gameObject.SetActive(true);
    }

    public override void DisconnectAnimal()
    {
        base.DisconnectAnimal();

        _textAnim.gameObject.SetActive(false);
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
        popup.SettingOnOff(Define.EBuildPopUpType.PopUpButton);
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

    public void Harvest()
    {
        IsHarvest?.Invoke();
    }

}
