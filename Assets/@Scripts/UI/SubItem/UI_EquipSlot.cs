using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlot : UI_Base
{
    #region Enum
    enum GameObjects
    {
 
    }

    enum Buttons
    {
    }

    enum Texts
    {

    }

    enum Images
    {
        EquipImage,
        CharacterOwnerImage,
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

        gameObject.BindEvent(OnClickEquipment);
        return true;
    }


    public void SetInfo(Equipment equipment)
    {
        if (equipment == null)
            return;

        _equipment = null;

        _equipment = equipment;


       // GetImage((int)Images.EquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        //GetImage((int)Images.CharacterOwnerImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquippedByCharacterId);

    }



    private void OnClickEquipment()
    {
        UI_EquipmentInfoPopup popup = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        popup.SetInfo(_equipment);
    }

}
