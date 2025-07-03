using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildingPopup : UI_Popup
{
    enum GameObjects
    {
        Content
    }

    enum Buttons
    {
        BackgroundCloseButton,
        UpgreadeButton,
    }
    enum Texts { CurrentLevelText, NextLevelText, LevelUpCost }
    enum Images { Building }
    
    private CookingBuilding _targetBuilding;
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundCloseButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.UpgreadeButton).gameObject.BindEvent(UpgreadeButton);

        SetInfo();
        return true;
    }

    private void UpgreadeButton()
    {
        _targetBuilding.Upgrade();
        SetInfo();
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    


    public void SetTarget(CookingBuilding building)
    {
        _targetBuilding = building;
    }

    public void SetInfo()
    {
        GetText((int)Texts.CurrentLevelText).text = _targetBuilding.CurrentLevel.ToString();
        GetText((int)Texts.NextLevelText).text = (_targetBuilding.CurrentLevel+1).ToString();
    }
    
}
