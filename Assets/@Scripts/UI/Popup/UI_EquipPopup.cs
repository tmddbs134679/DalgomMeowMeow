using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        EquipGroupObject
    }

    enum Buttons
    {
        ExitButton,
    }

    enum Texts
    {

    }

    enum Toggles
    {
        AllToggle,
        HatToggle,
        AccessoryToggle,
        BagToggle
    }
    #endregion

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
        BindToggle(typeof(Toggles));  
        
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetToggle((int)Toggles.AllToggle).gameObject.BindEvent(OnClickAllToggle);
        GetToggle((int)Toggles.HatToggle).gameObject.BindEvent(OnClickHatToggle);
        GetToggle((int)Toggles.AccessoryToggle).gameObject.BindEvent(OnClickAccessoryToggle);
        GetToggle((int)Toggles.BagToggle).gameObject.BindEvent(OnClickBagToggle);

        OnClickAllToggle();

        return true;
    }

    private void OnClickAllToggle()
    {
        GetObject((int)GameObjects.EquipGroupObject).DestroyChilds();

        List<Equipment> equipments = Managers.Game.OwnedEquipments;

        foreach (Equipment equipment in equipments)
        {
            UI_EquipSlot slot = Managers.UI.MakeSubItem<UI_EquipSlot>(GetObject((int)GameObjects.EquipGroupObject).transform);
            slot.SetInfo(equipment);
        }

    }

    private void OnClickHatToggle()
    {

    }

    private void OnClickAccessoryToggle()
    {

    }


    private void OnClickBagToggle()
    {
      
    }

    private void OnClickExitButton()
    {
        Managers.UI.CloseAllPopupUI();
    }


    private void Refresh()
    {
        GetObject((int)GameObjects.EquipGroupObject).DestroyChilds();
    }
}
