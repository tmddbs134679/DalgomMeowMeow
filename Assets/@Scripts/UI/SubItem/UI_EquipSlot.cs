using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        gameObject.BindEvent(OnClickEquipment);
        gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        return true;
    }


    public void SetInfo(Equipment equipment)
    {
        if (equipment == null)
            return;

        _equipment = null;

        _equipment = equipment;

        Sprite spr = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipImage).sprite = spr;

        // 캐릭터 오너 스프라이트 표시 (DataId 기준)
        if (!string.IsNullOrEmpty(_equipment.EquippedByCharacterId))
        {
            Character owner = Managers.Game.Characters.Find(c => c.UniqueId == _equipment.EquippedByCharacterId);
            if (owner != null && !string.IsNullOrEmpty(owner.Data?.IconLabel))
            {
                GetImage((int)Images.CharacterOwnerImage).sprite = Managers.Resource.Load<Sprite>(owner.Data.IconLabel);
            }
            else
            {
                GetImage((int)Images.CharacterOwnerImage).sprite = null;
            }
        }
        else
        {
            GetImage((int)Images.CharacterOwnerImage).sprite = null;
        }
    }



    private void OnClickEquipment()
    {
        UI_EquipmentInfoPopup popup = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        popup.SetInfo(_equipment);
    }

}
