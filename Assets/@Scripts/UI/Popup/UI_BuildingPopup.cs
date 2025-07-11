using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildingPopup : UI_Popup
{
    enum GameObjects
    {
        Content,
        Pivot
    }

    enum Buttons
    {
        BackgroundCloseButton,
        UpgreadeButton,
    }
    enum Texts { CurrentLevelText, NextLevelText, LevelUpCost }
    enum Images { Building }

    private BuildingBase _targetBuilding;
    public GameObject target;
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
        Camera.main.transform.LookAt(target.transform);
       // GetObject((int)GameObjects.Pivot).transform
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



    public void SetTarget(BuildingBase building)
    {
        _targetBuilding = building;
    }

    public void SetInfo()
    {
        if (_targetBuilding == null) return;

        int nextLevel = _targetBuilding.CurrentLevel + 1;
        var key = (_targetBuilding.BuildingData.Id.ToString(), nextLevel);
        if (Managers.Data.BuildingLevelDic.TryGetValue(key, out var levelData))
        {
            GetText((int)Texts.LevelUpCost).text = $"{levelData.UpgradeCost}";
            GetText((int)Texts.NextLevelText).text = (_targetBuilding.CurrentLevel + 1).ToString();
        }
        else
        {
            GetText((int)Texts.LevelUpCost).text = "Max";
            GetText((int)Texts.NextLevelText).text = "Max";
        }
        GetText((int)Texts.CurrentLevelText).text = _targetBuilding.CurrentLevel.ToString();
    }
    
    
    
    public void SetPivot(GameObject go)
    {
        target = go;
    }
}
