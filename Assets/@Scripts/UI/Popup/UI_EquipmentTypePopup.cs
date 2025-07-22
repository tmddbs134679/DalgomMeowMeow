using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EquipmentTypePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        CharacterScrollObject
    }

    enum Buttons
    {
        BackgroundButton,
        EquipButton,
        UnEquipButton
    }

    enum Texts
    {
        CharacterNameText,
    }

    enum Images
    {
        CharacterProfileImage,
    }

    #endregion

    AICharacter _aicharacter;
    Character _character;
    Equipment _equipment;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        Managers.UI.OnCharacterChange += RefreshCharacterProfile; 
    }

    private void OnDestroy()
    {
        Managers.UI.OnCharacterChange -= RefreshCharacterProfile;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));  

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.UnEquipButton).gameObject.BindEvent(OnClickUnEquipButton);
        GetButton((int)Buttons.UnEquipButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();




        return true;
    }

    private void OnClickUnEquipButton()
    {
        Clear();
        Managers.Equipment.UnEquipItem(_character, _equipment);
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickBackgroundButton()
    {
        Clear();
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickEquipButton()
    {
      
        Clear();
        Managers.Equipment.EquipItem(_character, _equipment);

        Managers.UI.ClosePopupUI(this);
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;

 
        if (!string.IsNullOrEmpty(_equipment.EquippedByCharacterId))
        {
            List<Character> characterList = Managers.Game.Characters;

            // 장착 중인 캐릭터 찾기
            Character equippedChar = characterList.Find(c => c.UniqueId == _equipment.EquippedByCharacterId);
            if (equippedChar != null)
            {
                RefreshCharacterProfile(equippedChar);
            }
        }


        Refresh();
    }

    void Refresh()
    {
        List<Character> characterList = Managers.Game.Characters;

        foreach (Character character in characterList)
        {
            UI_CharacterTypeSlot slot = Managers.UI.MakeSubItem<UI_CharacterTypeSlot>(GetObject((int)GameObjects.CharacterScrollObject).transform);
            slot.SetInfo(character, _equipment.EquipmentData.EquipmentType);
        }

        UpdateEquipButtonState();
    }

    private void RefreshCharacterProfile(Character character)
    {

        // 기존 미리보기 제거
        if (_aicharacter != null)
        {
            Managers.Resource.Destroy(_aicharacter.gameObject);
            _aicharacter = null;
        }

        _character = character;

        // 미리보기용 캐릭터 생성
        _aicharacter = Managers.Object.Spawn<AICharacter>(
            new Vector3(500, 500, 500), _character.DataId, null, true);

        // 해당 캐릭터가 장비의 주인인지 확인
        bool isEquippedByThisCharacter =
            _equipment.EquippedByCharacterId == _character.UniqueId;

        if (isEquippedByThisCharacter)
        {
            // 장비 주인이므로 실제 장비 상태대로 시각화
            Managers.Equipment.EquipCharacterVisual(_aicharacter, _character);
        }
        else
        {
            // 다른 캐릭터라면 미리보기로만 보여줌
            Managers.Equipment.EquipCharacterVisual(_aicharacter, _character, _equipment);
        }

        // 버튼 상태 갱신
        UpdateEquipButtonState();

        GetText((int)Texts.CharacterNameText).text = _character.Name;

    }

    private void UpdateEquipButtonState()
    {
        // 현재 캐릭터가 이 장비를 착용 중인지 확인
        bool isEquippedByCurrentCharacter =
            _equipment.EquippedByCharacterId == _character?.UniqueId;

        GetButton((int)Buttons.EquipButton).gameObject.SetActive(!isEquippedByCurrentCharacter);
        GetButton((int)Buttons.UnEquipButton).gameObject.SetActive(isEquippedByCurrentCharacter);
    }


    //레플리카 캐릭터 클리어
    private void Clear()
    {
        if(_aicharacter != null)
             Managers.Resource.Destroy(_aicharacter.gameObject);

        _aicharacter = null;    
    }
}
