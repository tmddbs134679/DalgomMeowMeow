using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CharacterShopPopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
       
    }

    enum Buttons
    {
        ProbabilityButton,
        OneTimeGachaButton,
        FiveTimeGachaButton
    }

    enum Texts
    {
        CharacterText,
    }
    #endregion



    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);
        PopupFadeInAnimation(GetText((int)Texts.CharacterText).gameObject);
        PopupFadeInAnimation(GetButton((int)Buttons.ProbabilityButton).gameObject);
        PopupFadeInAnimation(GetButton((int)Buttons.OneTimeGachaButton).gameObject);
        PopupFadeInAnimation(GetButton((int)Buttons.FiveTimeGachaButton).gameObject);

    }
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

        GetButton((int)Buttons.OneTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        return true;
    }
}
