using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildContent : UI_Popup
{
    enum GameObjects
    {
MoveOBJ,
    }

    enum Buttons
    {
        BackgroundCloseButton,
        InfoButton,
        PopUpButton,
    }
    enum Texts { }
    enum Images { }

    private CookingBuilding _cookingBuilding;
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundCloseButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.InfoButton).gameObject.BindEvent(OnClickInfoButton);
        GetButton((int)Buttons.PopUpButton).gameObject.BindEvent(OnClickPopupButton);
        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickInfoButton()
    {
        UI_InfoBuild popup = Managers.UI.ShowPopupUI<UI_InfoBuild>();
        popup.SetTarget(_cookingBuilding);
    }

    private void OnClickPopupButton()
    {
      UI_BuildingPopup popup =  Managers.UI.ShowPopupUI<UI_BuildingPopup>();
                popup.SetTarget(_cookingBuilding);
    }
    public void SetTarget(GameObject go)
    {
     //   GetObject((int)Buttons.BackgroundCloseButton).gameObject.transform.position = go.transform.position;
       _cookingBuilding= go.GetComponent<CookingBuilding>();
    }

}
