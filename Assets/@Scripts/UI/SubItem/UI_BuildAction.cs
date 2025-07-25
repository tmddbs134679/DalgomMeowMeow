using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UI_BuildAction : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        MoveUI,
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
        if(BuildingPlacer.Instance._isBuild)
                BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
        if (BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
            BuildingPlacer.Instance.AcceptLongPressBuild();
            if (!BuildingPlacer.Instance.isLongPressAcceptBuild && BuildingPlacer.Instance._isBuild)
            {

                BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
            }
        }
        else
        {
            if (BuildingPlacer.Instance.isSequenceBuild)
            {

                BuildingPlacer.Instance.AcceptSequenceBuild();
                BuildingPlacer.Instance.uI_BuildAction.SetActive(false);

            }
            else
            {
                BuildingPlacer.Instance.AcceptBuild();
                if(BuildingPlacer.Instance._isBuild)
                BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
            }

        }
        BuildingPlacer.Instance.isSequenceBuild = false;

    }
    private void CancelBuild()
    {
        if (BuildingPlacer.Instance.OnBuildingCancel != null)
        {
            // foreach (var d in BuildingPlacer.Instance.OnBuildingCancel.GetInvocationList())
            // {
            //     Debug.Log($"구독자: {d.Method.Name}, 소유 클래스: {d.Target}");
            // }
        }
        BuildingPlacer.Instance.isSequenceBuild = false;
        BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
        BuildingPlacer.Instance.CancelBuild();
        BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
    }

    private void RemoveBuild()
    {
        if (BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
            BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
            BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
            BuildingPlacer.Instance.RemoveBuild();
        }
    }
    private void Update()
    {
        if (BuildingPlacer.Instance.tempDraggleOBJ != null && this.gameObject.activeSelf)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, BuildingPlacer.Instance.tempDraggleOBJ.transform.position);
GetObject((int)GameObjects.MoveUI).transform.position=screenPos;
          //  this.gameObject.GetComponent<RectTransform>().position = screenPos;
        }
    }

    public void SetActive(bool istrue)
    {
        this.gameObject.SetActive(istrue);
    }
}
