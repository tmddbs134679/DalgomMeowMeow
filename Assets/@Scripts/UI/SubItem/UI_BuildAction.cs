using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildAction : UI_Popup
{
    #region Enum
    enum GameObjects
    {
    }

    enum Buttons
    {
        AcceptButton,
        CancelButton,
        RemoveButton,
    }

    enum Texts
    {

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

        GetButton((int)Buttons.AcceptButton).gameObject.BindEvent(AcceptBuild);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuild);
        GetButton((int)Buttons.RemoveButton).gameObject.BindEvent(RemoveBuild);

        return true;
    }
    private void AcceptBuild()
    {
        if (BuildingPlacer.Instance.isLongPressAcceptBuild) BuildingPlacer.Instance.OnBuildingAccept?.Invoke();
        if (BuildingPlacer.Instance.isLongPressAcceptBuild) Managers.UI.CloseAllPopupUI();
        if (BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
            BuildingPlacer.Instance.AcceptLongPressBuild();
        }
        else
        {
            BuildingPlacer.Instance.AcceptBuild();
        }
    }
    private void CancelBuild()
    {
        BuildingPlacer.Instance.OnBuildingCancel?.Invoke();//UI끄기 이벤트
        BuildingPlacer.Instance.CancelBuild();
    }

    private void RemoveBuild()
    {
       if (BuildingPlacer.Instance.isLongPressAcceptBuild) BuildingPlacer.Instance.RemoveBuild();
    }
    private void Update()
    {
        if (BuildingPlacer.Instance.tempDraggleOBJ != null && this.gameObject.activeSelf)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, BuildingPlacer.Instance.tempDraggleOBJ.transform.position);
            this.gameObject.GetComponent<RectTransform>().position = screenPos;
        }
    }

    public void SetActive(bool istrue)
    {
        this.gameObject.SetActive(istrue);
    }
}
