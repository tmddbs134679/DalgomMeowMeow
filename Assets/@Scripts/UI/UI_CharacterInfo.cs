using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterInfo : UI_Base
{

    #region Enum
    enum GameObjects
    {
       
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
        CharacterImage
    }
    #endregion


    Character _characterInfo;

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
        BindImage(typeof(Images));

        gameObject.BindEvent(OnClickObjectButton);
        return true;
    }

    private void OnClickObjectButton()
    {
        UI_ProfilePopup profile = Managers.UI.ShowPopupUI<UI_ProfilePopup>();
        profile.SetInfo(_characterInfo);
    }

    public void SetInfo(Character character)
    {
        _characterInfo = character;
        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(_characterInfo.Data.IconLabel);
        GetText((int)Texts.CharacterName).text = _characterInfo.Data.Name;
    }
}
