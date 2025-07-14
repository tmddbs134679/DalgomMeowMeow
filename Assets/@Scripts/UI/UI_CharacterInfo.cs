using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
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
        CharacterImage,
        HatImage,
        AccessoryImage,
        BagImage
    }
    #endregion


    Character _character;
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

    public void SetInfo(Character character)
    {
        _character = character;

        // 캐릭터 이름 & 이미지
        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        GetText((int)Texts.CharacterName).text = _character.Name;

        // Notify 체크
        CheckNotify();

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

                 icon = Managers.Resource.Load<Sprite>("Empty.sprite"); // 이건 상황에 맞게 수정
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
}
