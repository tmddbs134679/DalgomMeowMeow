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
    }

    enum Texts
    {
        CharacterNameText,

    }

    enum Images
    {
        CharacterImage,
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
        return true;
    }

    private void OnClickBackgroundButton()
    {
        Clear();
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickEquipButton()
    {
        Clear();
        Managers.Debug.Log("장비 착용", EDebugType.UI);
        Managers.UI.ClosePopupUI(this);
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;

        if (_equipment.EquippedByCharacterId != null)
        {
            //캐릭터 스폰
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
    }

    private void RefreshCharacterProfile(Character character)
    {

        if(_aicharacter != null)
        {
            Managers.Resource.Destroy(_aicharacter.gameObject);
            _aicharacter = null;
        }
  

        _character = character;

        _aicharacter = Managers.Object.Spawn<AICharacter>(new Vector3(500, 500, 500), _character.DataId, true);
        //추가 장비 장착


       // Managers.Game.ApplyEquippedPreview(_aicharacter, _character, _equipment);

        GetText((int)Texts.CharacterNameText).text = _character.Data.Name;

    }


    //레플리카 캐릭터 클리어
    private void Clear()
    {
        if(_aicharacter != null)
             Managers.Resource.Destroy(_aicharacter.gameObject);

        _aicharacter = null;    
    }
}
