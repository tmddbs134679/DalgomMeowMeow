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
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        Refresh();

        return true;
    }


    private void OnClickExitButton()
    {
        Managers.UI.CloseAllPopupUI();
    }


    private void Refresh()
    {
        List<Equipment> equipments = Managers.Game.OwnedEquipments;

        foreach (Equipment equipment in equipments)
        {
            UI_EquipSlot slot = Managers.UI.MakeSubItem<UI_EquipSlot>(GetObject((int)GameObjects.EquipGroupObject).transform);
            slot.SetInfo(equipment);
        }

    }
}
