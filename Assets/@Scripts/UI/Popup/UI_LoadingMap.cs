using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_LoadingMap : UI_Popup
{

    #region Enum
    enum GameObjects
    {
    }

    enum Buttons
    {
        BackgroundButton,

    }

    enum Texts
    {
        ProgressText,
    }

    enum Images
    {
    }
    #endregion

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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        return true;
    }


    private void OnEnable()
    {

    }

    private void OnClickBackgroundButton()
    {

    }

    public void SetProgress(int current, int total)
    {
        
        if (GetText((int)Texts.ProgressText) != null)
            GetText((int)Texts.ProgressText).text = $"{current} / {total}";
    }

    public void SetPercentage(float ratio)
    {
        if (GetText((int)Texts.ProgressText) != null)
            GetText((int)Texts.ProgressText).text = $"{Mathf.RoundToInt(ratio * 100)}%";
    }
}
