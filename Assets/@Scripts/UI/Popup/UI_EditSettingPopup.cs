using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EditSettingPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,

    }

    enum Buttons
    {
        BackgroundButton,
        SoundEffectOnButton,
        SoundEffectOffButton,
        BgmEffectOnButton,
        BgmEffectOffButton
    }

    enum Texts
    {

    }
    #endregion


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

        GetButton((int)Buttons.SoundEffectOnButton).gameObject.BindEvent(OnClickSoundOffButton);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.BindEvent(OnClickSoundOnButton);

        GetButton((int)Buttons.BgmEffectOnButton).gameObject.BindEvent(OnClickBgmOffButton);
        GetButton((int)Buttons.BgmEffectOffButton).gameObject.BindEvent(OnClickBgmOnButton);


        if (Managers.Game.EffectSoundOn == false)
        {
            OnClickSoundOffButton();
        }
        else
        {
            OnClickSoundOnButton();
        }

        if (Managers.Game.BGMOn == false)
        {
            OnClickBgmOffButton();
        }
        else
        {
            OnClickBgmOnButton();
        }

    

        return true;
    }


    private void OnClickSoundOnButton()
    {
        Managers.Game.EffectSoundOn = true;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(false);
    }

    private void OnClickSoundOffButton()
    {
        Managers.Game.EffectSoundOn = true;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(true);
    }



    private void OnClickBackgroundButton()
    {
        gameObject.SetActive(false);
    }

    private void OnClickBgmOnButton()
    {
        Managers.Game.BGMOn = true;
        GetButton((int)Buttons.BgmEffectOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.BgmEffectOffButton).gameObject.SetActive(false);
    }

    private void OnClickBgmOffButton()
    {
        Managers.Game.BGMOn = false;
        GetButton((int)Buttons.BgmEffectOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.BgmEffectOffButton).gameObject.SetActive(true);
    }




}
