using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Define;

public class UI_CharacterInfo : UI_Base
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
        CharacterName
    }

    enum Images
    {
        CharacterImageBackground,
        HatImageObject,
        AccessorymageObject,
        BagImageObject,
        CharacterImage,
        HatImage,
        AccessoryImage,
        BagImage
    }
    #endregion


    Character _character;
    ScrollRect _scrollRect;
    bool _isDrag = false;

    List<EEquipmentType> displayOrder = new()
    {
        EEquipmentType.Hat,
        EEquipmentType.Accessory,
        EEquipmentType.Bag
    };
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Managers.UI.OnCharacterChange += UpdateCharacterText;
    }
    private void OnDestroy()
    {
        Managers.UI.OnCharacterChange -= UpdateCharacterText;
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        gameObject.BindEvent(OnClickObjectButton);

        GetObject((int)GameObjects.NewTextObject).gameObject.SetActive(false);
        return true;
    }

    private void OnClickObjectButton()
    {
        UI_ProfilePopup profile = Managers.UI.ShowPopupUI<UI_ProfilePopup>();
        profile.SetInfo(_character);

        _character.IsConfirmed = true;

        GetObject((int)GameObjects.NewTextObject).SetActive(!_character.IsConfirmed);

        Managers.Game.SaveGame();
    }

    public void SetInfo(Character character, ScrollRect scrollrect)
    {
        _character = character;
        _scrollRect = scrollrect;

        // 캐릭터 이름 & 이미지
        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        GetText((int)Texts.CharacterName).text = _character.Name;

        // Notify 체크
        CheckNotify();

        // 캐릭터 종류별 배경 변경
        if (_character.UniqueId[0] == 'B')
        {
            GetImage((int)Images.CharacterImageBackground).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground1.sprite");
            GetImage((int)Images.AccessorymageObject).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground1.sprite");
            GetImage((int)Images.BagImageObject).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground1.sprite");
            GetImage((int)Images.HatImageObject).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground1.sprite");
        }
        else
        {
            GetImage((int)Images.CharacterImageBackground).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground4.sprite");
            GetImage((int)Images.AccessorymageObject).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground4.sprite");
            GetImage((int)Images.BagImageObject).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground4.sprite");
            GetImage((int)Images.HatImageObject).sprite = Managers.Resource.Load<Sprite>("UI_CharacterBackground4.sprite");
        }

        // 장비 이미지 초기화
        foreach (EEquipmentType type in displayOrder)
        {
            Sprite icon;

            if (_character.EquippedItems.TryGetValue(type, out var equip))
            {
                // 장비가 있으면 해당 장비 아이콘
                icon = Managers.Resource.Load<Sprite>(equip.EquipmentData.SpriteName);
            }
            else
            {
                // 장비 없으면 기본 슬롯 이미지

                 icon = Managers.Resource.Load<Sprite>("AlphaBackground.sprite"); // 이건 상황에 맞게 수정
            }

            // 슬롯에 맞게 이미지 넣기
            switch (type)
            {
                case EEquipmentType.Hat:
                    GetImage((int)Images.HatImage).sprite = icon;
                    break;
                case EEquipmentType.Accessory:
                    GetImage((int)Images.AccessoryImage).sprite = icon;
                    break;
                case EEquipmentType.Bag:
                    GetImage((int)Images.BagImage).sprite = icon;
                    break;
            }
        }
    }

    private void UpdateCharacterText(Character character)
    {
        if (_character == null)
            return;

        GetText((int)Texts.CharacterName).text = _character.Name;
    }


    private void CheckNotify()
    {
        if(!_character.IsConfirmed)
            GetObject((int)GameObjects.NewTextObject).gameObject.SetActive(true);
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
