using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {

    }

    enum Buttons
    {
        ExitButton,
    }

    enum Texts
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
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        return true;
    }


    private void OnClickExitButton()
    {
        Managers.UI.CloseAllPopupUI();
    }

}
