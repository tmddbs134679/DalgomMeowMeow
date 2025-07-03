using System;
using System.Collections;
using System.Collections.Generic;
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

    Equipment _equipment;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickEquipButton()
    {
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

}
