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
        CancelButton
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

        return true;
    }
    private void AcceptBuild()
    {
        BuildingPlacer.Instance.AcceptBuild();
    }
    private void CancelBuild()
    {
        Managers.Resource.Destroy(gameObject);
        BuildingPlacer.Instance.CancelBuild();
    }
    private void Update()
    {
        if (BuildingPlacer.Instance.tempDraggleOBJ != null && this.gameObject.activeSelf)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, BuildingPlacer.Instance.tempDraggleOBJ.transform.position);
            this.gameObject.GetComponent<RectTransform>().position = screenPos;
        }
    }
}
