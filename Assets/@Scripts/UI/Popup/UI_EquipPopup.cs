using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
public class UI_EquipPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
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

    EEquipmentType _currentType;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        ToggleInit();
       
        OnClickTypeToggle();

        Managers.Game.EquipInfoChanged += Refresh;
    }

    private void OnDestroy()
    {
        Managers.Game.EquipInfoChanged -= Refresh;
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
        GetToggle((int)Toggles.AllToggle).gameObject.BindEvent(() => OnClickTypeToggle());
        GetToggle((int)Toggles.HatToggle).gameObject.BindEvent(()=> OnClickTypeToggle(EEquipmentType.Hat));
        GetToggle((int)Toggles.AccessoryToggle).gameObject.BindEvent(() => OnClickTypeToggle(EEquipmentType.Accessory));
        GetToggle((int)Toggles.BagToggle).gameObject.BindEvent(() => OnClickTypeToggle(EEquipmentType.Bag));

        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.AllToggle).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.HatToggle).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.AccessoryToggle).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.BagToggle).gameObject.GetOrAddComponent<UI_ButtonAnimation>();


        return true;
    }

    void ToggleInit()
    {
        GetToggle((int)Toggles.AllToggle).isOn = true;
        GetToggle((int)Toggles.HatToggle).isOn = false;
        GetToggle((int)Toggles.AccessoryToggle).isOn = false;
        GetToggle((int)Toggles.BagToggle).isOn = false;
    }
    //private void OnClickAllToggle()
    //{
    //    GetObject((int)GameObjects.EquipGroupObject).DestroyChilds();

    //    List<Equipment> equipments = Managers.Game.OwnedEquipments;

    //    foreach (Equipment equipment in equipments)
    //    {
    //        UI_EquipSlot slot = Managers.UI.MakeSubItem<UI_EquipSlot>(GetObject((int)GameObjects.EquipGroupObject).transform);
    //        slot.SetInfo(equipment);
    //    }

    //}

    //null일떈 전부 보이게 설정함.
    private void OnClickTypeToggle(EEquipmentType? type = EEquipmentType.None)
    {
        _currentType = (EEquipmentType)type;
        GetObject((int)GameObjects.EquipGroupObject).DestroyChilds();

        List<Equipment> equipments = Managers.Game.OwnedEquipments;

        if (type == EEquipmentType.None)
            equipments.Sort((a, b) => string.Compare(a.key, b.key, StringComparison.Ordinal));

        foreach (Equipment equipment in equipments)
        {
            if (type != EEquipmentType.None && equipment.EquipmentData.EquipmentType != type.Value)
                continue;

            UI_EquipSlot slot = Managers.UI.MakeSubItem<UI_EquipSlot>(GetObject((int)GameObjects.EquipGroupObject).transform);
            slot.SetInfo(equipment);
        }

    }
    private void OnClickExitButton()
    {

        //Nofity Check
        (Managers.UI.SceneUI as UI_GameScene).CheckNotify();

        gameObject.SetActive(false);
        _currentType = Define.EEquipmentType.None;


    }


    private void Refresh()
    {
        GetObject((int)GameObjects.EquipGroupObject).DestroyChilds();
        OnClickTypeToggle(_currentType);


    }
}
