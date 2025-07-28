using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
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

    ScrollRect _scrollRect;
    bool _isDrag = false;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        gameObject.BindEvent(OnClickObject);
        gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        return true;
    }

    void OnClickObject()
    {
        Managers.Sound.PlayButtonClick();

        Managers.Debug.Log("클릭했음", EDebugType.UI);
        Managers.UI.OnCharacterChange?.Invoke(_character);
    }

    Character _character;
    public void SetInfo(Character character, EEquipmentType type, ScrollRect scrollrect)
    {
        _character = character;
        _scrollRect = scrollrect; 

        foreach (string itemId in _character.EquippedItemIds)
        {
            // 보유 장비 목록에서 ID로 Equipment 찾기
            Equipment equipment = Managers.Game.OwnedEquipments.Find(e => e.UniqueId == itemId);
            if (equipment != null && equipment.EquipmentData.EquipmentType == type)
            {
                GetImage((int)Images.EquipImage).sprite = Managers.Resource.Load<Sprite>(equipment.EquipmentData.SpriteName);
            }
            else
            {
                GetImage((int)Images.EquipImage).sprite = Managers.Resource.Load<Sprite>("AlphaBackground.sprite");
            }
        }

       //리소스 없음 아직
       GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
    }

    public void OnDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);

    }

}
