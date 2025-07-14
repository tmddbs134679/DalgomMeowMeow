using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildingPopup : UI_Popup
{
    enum GameObjects
    {
        Pivot,
    }

    enum Buttons
    {
        BackgroundCloseButton,
        UpgreadeButton,
    }
    enum Texts { CurrentLevelText, NextLevelText, LevelUpCost }
    enum Images { Building }

    private BuildingBase _targetBuilding;
    public GameObject target;



    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundCloseButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.UpgreadeButton).gameObject.BindEvent(UpgreadeButton);

        SetInfo();


        //         Vector3 camForward = Camera.main.transform.forward;
        //     Vector3 camRight = Camera.main.transform.right;
        //     Vector3 camUp = Camera.main.transform.up;

        //         float distanceToTarget = Vector3.Dot(target.transform.position - Camera.main.transform.position, camForward);

        //    pivotX = (GetObject((int)GameObjects.Pivot).transform.position.x / 2400f)+0.5f;  // = 0.2842
        // pivotY = (GetObject((int)GameObjects.Pivot).transform.position.y / 1080f)+0.5f;  // = 0.4565
        //         pivotScreenPos = new Vector2(pivotX,pivotY);



        //         Vector3 pivotOffset =
        //             camRight * (pivotScreenPos.x - 0.5f) * 20f +
        //             camUp * (pivotScreenPos.y - 0.5f) * 20f;

        //         Vector3 newCamPos = target.transform.position - camForward * distanceToTarget + pivotOffset;

        //         newCamPos.y = Camera.main.transform.position.y;

        //         Camera.main.transform.position = newCamPos;
        //         Camera.main.transform.rotation = Quaternion.Euler(45, 45, 0);
        FocusCameraOnPivot();

        return true;
    }

    private void UpgreadeButton()
    {
        _targetBuilding.Upgrade();
        SetInfo();
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }



    public void SetTarget(BuildingBase building)
    {
        _targetBuilding = building;
    }

    public void SetInfo()
    {
        if (_targetBuilding == null) return;

        int nextLevel = _targetBuilding.CurrentLevel + 1;
        var key = (_targetBuilding.BuildingData.Id.ToString(), nextLevel);
        if (Managers.Data.BuildingLevelDic.TryGetValue(key, out var levelData))
        {
            GetText((int)Texts.LevelUpCost).text = $"{levelData.UpgradeCost}";
            GetText((int)Texts.NextLevelText).text = (_targetBuilding.CurrentLevel + 1).ToString();
        }
        else
        {
            GetText((int)Texts.LevelUpCost).text = "Max";
            GetText((int)Texts.NextLevelText).text = "Max";
        }
        GetText((int)Texts.CurrentLevelText).text = _targetBuilding.CurrentLevel.ToString();
    }



    public void SetPivot(GameObject go)
    {
        target = go;
    }

    private void FocusCameraOnPivot()
    {
                Vector3 camForward = Camera.main.transform.forward;

        // 카메라와 타겟 사이 거리
        float distanceToTarget = Vector3.Dot(target.transform.position - Camera.main.transform.position, camForward);

        // 타겟 위치에서 카메라 방향으로 역산
        Vector3 newCamPos = target.transform.position - camForward * distanceToTarget;

        // 카메라 위치 이동
        Camera.main.transform.position = new Vector3(newCamPos.x + 6.05f, Camera.main.transform.position.y, newCamPos.z - 3.38f);
    }
}
