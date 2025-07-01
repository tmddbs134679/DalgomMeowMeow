using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UI_ProfilePopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        EquippedGroupObject,
    }

    enum Buttons
    {
        NextCharacterButton,
        PrevCharacterButton,
        ExitButton,
    }

    enum Texts
    {
        CharacterNameText
    }

    enum Images
    {
        Image
    }
    #endregion

    Character _character;
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
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.PrevCharacterButton).gameObject.BindEvent(() => OnClickChangeButton(-1));
        GetButton((int)Buttons.NextCharacterButton).gameObject.BindEvent(() => OnClickChangeButton(1));

        Refresh();

        return true;
    }

    private void OnClickChangeButton(int dir)
    {
       // Todo : 다음 캐릭터, 이전 캐릭터

    }

    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }



    private void SetInfo(Character character)
    {
        //_character = character;
        //GetImage((int)Images.Image).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        //GetText((int)Texts.CharacterNameText).text = _character.Data.Name;

        //foreach(string quip in _character.EquippedItemIds)
        //{
        //    UI_EquipItem item = Managers.UI.MakeSubItem<UI_EquipItem>(GetObject((int)GameObjects.EquippedGroupObject).transform);
        //    item.SetInfo(quip);
        //}
        
        //Todo : 모자, 가방, 악세서리 아이템 나오면 연결 MakeSubItem말고 고정값들
    }
    private void Refresh()
    {
        if(_character != null)
             _character = null;

        _character =  Managers.Game.Characters[0];

        SetInfo(_character);
    }
}
