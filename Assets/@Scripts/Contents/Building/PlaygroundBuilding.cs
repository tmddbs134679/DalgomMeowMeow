using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundBuilding : BuildingBase
{
    private void Awake()
    {
        _textAnim = Managers.UI.ShowPopupUI<UI_TextAnimation>();

    }


    protected override void Start()
    {
        base.Start();

        _textAnim.gameObject.SetActive(false);
        _textAnim.SetInfo(Define.EBuildingType.Playing, transform.position);
    }


    private void OnDestroy()
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


    public override void Produce()
    {
        //노는 애니메이션 재생
    }

    public override void OnClick()
    {
        
    }
}
