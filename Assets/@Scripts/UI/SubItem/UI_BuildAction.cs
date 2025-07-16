using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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

    public bool islimitBuildCount;
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
//여기서도 판별해야하는데..
        if (BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
            BuildingPlacer.Instance.AcceptLongPressBuild();
            if (!BuildingPlacer.Instance.isLongPressAcceptBuild && BuildingPlacer.Instance._isBuild)
            {
                BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
            }
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
        if (BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
                    BuildingPlacer.Instance.OnBuildingCancel?.Invoke();//UI끄기 이벤트
            BuildingPlacer.Instance.RemoveBuild();
        }
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
