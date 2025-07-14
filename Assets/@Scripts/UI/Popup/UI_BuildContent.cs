using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildContent : UI_Popup
{
    enum GameObjects
    {
        MoveUIPanel,
        sfgwegeg,
    }

    enum Buttons
    {
        BackgroundCloseButton,
        InfoButton,
        PopUpButton,
    }
    enum Texts { }
    enum Images { }

    private BuildingBase _cookingBuilding;
    private GameObject _tempObj;
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

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_tempObj.transform.position);
        GetObject((int)GameObjects.MoveUIPanel).GetComponent<RectTransform>().position = screenPos;
        return true;
    }

    void OnEnable()
    {
        Managers.UI.OnLongPress += CloseUI;
    }
    void OnDisable()
    {
        Managers.UI.OnLongPress -= CloseUI;
    }
    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickInfoButton()
    {
        Managers.UI.ClosePopupUI(this);
        UI_InfoBuild popup = Managers.UI.ShowPopupUI<UI_InfoBuild>();
        popup.SetTarget(_cookingBuilding);
    }

    private void OnClickPopupButton()
    {
        Managers.UI.ClosePopupUI(this);
        UI_BuildingPopup popup = Managers.UI.ShowPopupUI<UI_BuildingPopup>();
        popup.SetTarget(_cookingBuilding);
        popup.SetPivot(_tempObj);
    }
    public void SetTarget(GameObject go)
    {
        _tempObj = go;
        _cookingBuilding = go.GetComponent<BuildingBase>();
    }

    public void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
