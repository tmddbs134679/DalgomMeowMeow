using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// 건설할 때 최종적으로 결정하는 UI
/// </summary>
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
        RemoveRoadButton,
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
    void OnEnable()
    {
        ButtonSetAtive();
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
        GetButton((int)Buttons.RemoveRoadButton).gameObject.BindEvent(RemoveRoadBuild);

        return true;
    }

    //설치 버튼
    private void AcceptBuild()
    {
        BuildingPlacer.Instance.CanPlaceBuilding();
        if (BuildingPlacer.Instance._isBuild)
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
                if (BuildingPlacer.Instance._isBuild)
                    BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
            }

        }
        BuildingPlacer.Instance.isSequenceBuild = false;
        BuildingPlacer.Instance.isSequenceRemove = false;
    }
    //취소 버튼
    private void CancelBuild()
    {
        BuildingPlacer.Instance.isSequenceBuild = false;
        BuildingPlacer.Instance.isSequenceRemove = false;
        BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
        BuildingPlacer.Instance.CancelBuild();
        BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
    }
    //건물,도로삭제 버튼
    private void RemoveBuild()
    {
        //롱프레스일때만 버튼 클릭하게 하였지만 애초에 버튼이 안보이면 좋겠음
        if (BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
            BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
            BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
            BuildingPlacer.Instance.RemoveBuild();
        }

    }
    //도로삭제 버튼
    private void RemoveRoadBuild()
    {
        if (BuildingPlacer.Instance.isSequenceRemove)
        {
            BuildingPlacer.Instance.OnBuildingCancel?.Invoke();
            BuildingPlacer.Instance.uI_BuildAction.SetActive(false);
            BuildingPlacer.Instance.RemoveRoad();
            BuildingPlacer.Instance.isSequenceBuild = false;
            BuildingPlacer.Instance.isSequenceRemove = false;
        }
    }
    //UI가 선택된 오브젝트 따라가게 하기
    private void Update()
    {
        if (BuildingPlacer.Instance.tempDraggleOBJ != null && this.gameObject.activeSelf)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, BuildingPlacer.Instance.tempDraggleOBJ.transform.position);
            GetObject((int)GameObjects.MoveUI).transform.position = screenPos;
            //  this.gameObject.GetComponent<RectTransform>().position = screenPos;
        }
    }
    //UI자체 hide,show
    public void SetActive(bool istrue)
    {
        this.gameObject.SetActive(istrue);
    }

    //버튼 UI Hide,show
    public void ButtonSetAtive()
    {
        if (!BuildingPlacer.Instance.isLongPressAcceptBuild)
        {
            GetButton((int)Buttons.RemoveButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.RemoveButton).gameObject.SetActive(true);
        }
        if (!BuildingPlacer.Instance.isSequenceRemove)
        {
            GetButton((int)Buttons.RemoveRoadButton).gameObject.SetActive(false);
            GetButton((int)Buttons.AcceptButton).gameObject.SetActive(true);
        }
        else
        {
            GetButton((int)Buttons.RemoveRoadButton).gameObject.SetActive(true);
            GetButton((int)Buttons.AcceptButton).gameObject.SetActive(false);
        }
    }
}
