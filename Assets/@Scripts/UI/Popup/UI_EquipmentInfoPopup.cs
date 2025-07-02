using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentInfoPopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        EquippedObject
    }

    enum Buttons
    {
        BackgroundButton,
        UnEquipButton,
        EquipButton
    }

    enum Texts
    {
        EquipmentText,
        EquipmentDescriptionText
    }

    enum Images
    {
        EquipmentImage,
        EquippedCharacterImage,


    }
    #endregion

    Equipment _equipment;

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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        return true;
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;
        GetText((int)Texts.EquipmentText).text = _equipment.EquipmentData.Name;
        GetText((int)Texts.EquipmentDescriptionText).text = _equipment.EquipmentData.Description;
       // GetImage((int)Images.EquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
    }
    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
