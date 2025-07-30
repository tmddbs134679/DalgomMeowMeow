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

    private void OnDestroy()
    {
        if (_textAnim != null)
            _textAnim.gameObject.SetActive(false);
    }
    public override void ConnectToAnimal(AICharacter animal)
    {
        base.ConnectToAnimal(animal);

        _textAnim.gameObject.SetActive(true);
        CurrentState = BuildingState.Producing;
    }

    public override void DisconnectAnimal()
    {
        base.DisconnectAnimal();

        _textAnim.gameObject.SetActive(false);
        CurrentState = BuildingState.Idle;
    }


    public override bool Init()
    {
        base.Init();
        return true;
    }
    public override void Produce()
    {
        // 상태 전이
        CurrentState = BuildingState.Producing;

    }
    public void Collect()
    {


    }
    public override void OnClick()
    {

    }
    
}
