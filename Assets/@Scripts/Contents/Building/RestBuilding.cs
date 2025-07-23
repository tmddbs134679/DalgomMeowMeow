using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;

    public GameObject collectIcon;

    private void Awake()
    {
        _textAnim = Managers.UI.ShowPopupUI<UI_TextAnimation>();

    }
    protected override void Start()
    {
        base.Start();

        _textAnim.gameObject.SetActive(false);
        _textAnim.SetInfo(Define.EBuildingType.Resting, transform.position);
    }

    private void OnDisable()
    {
        _textAnim.gameObject.SetActive(false);
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


    public override bool Init()
    {
        base.Init();
        // collectIcon.SetActive(false);
        return true;
    }
    public override void Produce()
    {
        Debug.Log("휴식 완료");


        // 상태 전이
        CurrentState = BuildingState.ReadyToCollect;

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
