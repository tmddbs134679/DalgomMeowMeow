using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_InfoBuild : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        ContentObject,
        EquippedObject
    }

    enum Buttons
    {
        BackgroundButton,

    }

    enum Texts
    {
        EquipmentText,
        EquipmentDescriptionText,
    }

    enum Images
    {
        EquipmentImage,
    }
    #endregion
  private BuildingBase _targetBuilding;
    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        return true;
    }


    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public void SetTarget(BuildingBase building)
    {
        _targetBuilding = building;
    }
}
