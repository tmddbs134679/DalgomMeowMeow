using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CheckOutRewardPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        RewardGroupObject
    }

    enum Buttons
    {
        BackgroundButton,
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
        Refresh();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        #endregion
        return true;
    }

    private void Refresh()
    {
        
    }
    
    public void SetInfo(int dayCount)
    {
        GameObject GroupObject = GetObject((int)GameObjects.RewardGroupObject);
        UI_CheckOutItem slot = Managers.UI.MakeSubItem<UI_CheckOutItem>(GroupObject.transform);
        slot.SetInfo(dayCount, false, true);
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
