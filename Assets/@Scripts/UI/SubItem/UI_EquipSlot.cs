using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlot : UI_Base
{
    #region Enum
    enum GameObjects
    {
        NewTextObject
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

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        gameObject.BindEvent(OnClickEquipment);
        gameObject.GetOrAddComponent<UI_ButtonAnimation>();

       gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
       gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
       gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        return true;
    }


    public void SetInfo(Equipment equipment, bool isReward, ScrollRect scrollRect = null)
    {
        if(scrollRect != null)
            _scrollRect = scrollRect;

        if (equipment == null)
            return;

        if (isReward)
            GetImage((int)Images.CharacterOwnerImage).gameObject.SetActive(false);


        _equipment = null;

        _equipment = equipment;


        GetObject((int)GameObjects.NewTextObject).SetActive(!_equipment.IsConfirmed);


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
            GetImage((int)Images.CharacterOwnerImage).sprite = Managers.Resource.Load<Sprite>("AlphaBackground.sprite");
        }
    }



    private void OnClickEquipment()
    {
        Managers.Sound.PlayButtonClick();

        UI_EquipmentInfoPopup popup = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        popup.SetInfo(_equipment);

        _equipment.IsConfirmed = true;

        GetObject((int)GameObjects.NewTextObject).SetActive(!_equipment.IsConfirmed);

        Managers.Game.SaveGame();
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
