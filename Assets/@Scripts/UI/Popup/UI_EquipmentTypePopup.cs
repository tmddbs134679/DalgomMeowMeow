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
        Managers.Game.UnEquipItem(_character, _equipment);
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
        Managers.Game.EquipItem(_character, _equipment);

        Managers.UI.ClosePopupUI(this);
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;

 
        if (!string.IsNullOrEmpty(_equipment.EquippedByCharacterId))
        {
            List<Character> characterList = Managers.Game.Characters;

            // 장착 중인 캐릭터 찾기
            Character equippedChar = characterList.Find(c => c.DataId == _equipment.EquippedByCharacterId);
            if (equippedChar != null)
            {
                RefreshCharacterProfile(equippedChar);
                equippedChar = _character;
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

        if(_aicharacter != null)
        {
            Managers.Resource.Destroy(_aicharacter.gameObject);
            _aicharacter = null;
        }

        _character = character;

        _aicharacter = Managers.Object.Spawn<AICharacter>(new Vector3(500, 500, 500), _character.DataId, null, true);
        //추가 장비 장착

        Managers.Game.EquipCharacterVisual(_aicharacter, _character, _equipment);

        GetText((int)Texts.CharacterNameText).text = _character.Data.Name;

    }

    private void UpdateEquipButtonState()
    {
        bool isEquipped = !string.IsNullOrEmpty(_equipment.EquippedByCharacterId);

        GetButton((int)Buttons.EquipButton).gameObject.SetActive(!isEquipped);
        GetButton((int)Buttons.UnEquipButton).gameObject.SetActive(isEquipped);
    }


    //레플리카 캐릭터 클리어
    private void Clear()
    {
        if(_aicharacter != null)
             Managers.Resource.Destroy(_aicharacter.gameObject);

        _aicharacter = null;    
    }
}
