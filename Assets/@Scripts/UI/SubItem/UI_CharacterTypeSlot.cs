using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Define;

public class UI_CharacterTypeSlot : UI_Base
{

    #region Enum
    enum GameObjects
    {
      
    }

    enum Buttons
    {
   
    }

    enum Images
    {
        CharacterImage,
        EquipImage
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

        gameObject.BindEvent(OnClickObject);
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        return true;
    }

    void OnClickObject()
    {
        Managers.Debug.Log("클릭했음", EDebugType.UI);
        Managers.UI.OnCharacterChange?.Invoke(_character);
    }

    Character _character;
    public void SetInfo(Character character, EEquipmentType type)
    {
        _character = character;

        foreach (string itemId in _character.EquippedItemIds)
        {
            // 보유 장비 목록에서 ID로 Equipment 찾기
            Equipment equipment = Managers.Game.OwnedEquipments.Find(e => e.key == itemId);
            if (equipment != null && equipment.EquipmentData.EquipmentType == type)
            {
                GetImage((int)Images.EquipImage).sprite = Managers.Resource.Load<Sprite>(equipment.EquipmentData.SpriteName);
            }
        }

       //리소스 없음 아직
       GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
    }
}
