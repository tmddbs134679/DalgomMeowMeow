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
        SlotButton,
    }
    enum Texts { }
    enum Images { }

    private BuildingBase _buildingBase;
    private GameObject _tempObj;
    private Define.EBuildPopUpType _type;
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
        GetButton((int)Buttons.SlotButton).gameObject.BindEvent(OnClickSlotButton);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_tempObj.transform.position);
        GetObject((int)GameObjects.MoveUIPanel).GetComponent<RectTransform>().position = screenPos;

        SelectButton();

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
        popup.SetTarget(_buildingBase);
    }

    private void OnClickPopupButton()
    {
        Managers.UI.ClosePopupUI(this);
        UI_BuildingPopup popup = Managers.UI.ShowPopupUI<UI_BuildingPopup>();
        popup.SetTarget(_buildingBase);
        popup.SetPivot(_tempObj);
    }

    private void OnClickSlotButton()
    {
        Managers.UI.ClosePopupUI(this);
        UI_SlotMachinePopup popup = Managers.UI.ShowPopupUI<UI_SlotMachinePopup>();
        popup.SetPivot(_tempObj);

        if (_buildingBase is SlotMachineBuilding slot)
        {
            popup.SetTarget(slot);
        }
        else
        {
            Debug.LogError("baseBuilding은 SlotMachineBuilding이 아님!");
        }

    }
    public void SetTarget(GameObject go)
    {
        _tempObj = go;
        _buildingBase = go.GetComponent<BuildingBase>();
    }

    public void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public void SettingOnOff(Define.EBuildPopUpType type)
    {
        _type = type;
    }

    private void SelectButton()
    {
        switch (_type)
        {
            case Define.EBuildPopUpType.PopUpButton:
                GetButton((int)Buttons.PopUpButton).gameObject.SetActive(true);
                GetButton((int)Buttons.SlotButton).gameObject.SetActive(false);
                break;
            case Define.EBuildPopUpType.SlotButton:
                GetButton((int)Buttons.PopUpButton).gameObject.SetActive(false);
                GetButton((int)Buttons.SlotButton).gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning($"[SettingOnOff] 알 수 없는 타입: {_type}");
                break;
        }
    }
}
