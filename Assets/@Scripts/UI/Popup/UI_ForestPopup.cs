using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ForestPopup : UI_Popup
{
    enum GameObjects
    {
        // Content
    }

    enum Buttons
    {
        BattleButton,
        Background
    }
    enum Texts { ForestTitleText, }

    enum Images
    {
        FirstEnemy,
        SecondEnemy,
        ThirdEnemy,
    }

    private StageSO stagedata;


    public override bool Init()
    {
        if (!base.Init()) return false;

        //BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BattleButton).gameObject.BindEvent(OnClickBattleButton);
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);

        GetStageData();

        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI();
    }

    private void OnClickBattleButton()
    {
        Managers.Scene.LoadScene(Define.EScene.Test_Battle);
    }

    private void GetStageData()
    {
        stagedata = StageDataManager.Instance.SetStage();
        switch (stagedata.EnemyID.Length) 
        {
            case 1:
                break;
            case 2:
                GetImage((int)Images.FirstEnemy).sprite = Managers.Resource.Load <Sprite>(stagedata.EnemyID[0]);
                GetImage((int)Images.SecondEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[1]);
                GetImage((int)Images.ThirdEnemy).gameObject.SetActive(false);
                break;
            case 3:
                GetImage((int)Images.FirstEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[0]);
                GetImage((int)Images.SecondEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[1]);
                GetImage((int)Images.ThirdEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[2]);
                break;
        }
    }
}
