using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UI_ForestPopup : UI_Popup
{
    enum GameObjects
    {
        Content,
        IconBackground
    }

    enum Buttons
    {
        BattleButton,
        Background,
        IconBackground
    }
    enum Texts 
    {
        ForestTitleText,
        Atk,
        Health,
    }

    enum Images
    {
        CharacterIcon,
        AtkIcon,
        HealthIcon,
        SkillIcon,
        Select,
        Select1,
        Select2,
        FirstEnemy,
        SecondEnemy,
        ThirdEnemy,
    }

    Character _character;
    

    private StageSO stagedata;


    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BattleButton).gameObject.BindEvent(OnClickBattleButton);
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.IconBackground).gameObject.BindEvent(OnClicCharacterIamage);
        GetCharacterInfo();
        //GetStageData();

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


    public void GetCharacterInfo()
    {
        List<int> characterList = new List<int>();

        characterList.Add(1);
        characterList.Add(2);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);
        characterList.Add(3);

        for (int i = 0; i < characterList.Count; i++)
        {
            int character = characterList[i];

            GameObject slot = Instantiate(GetObject((int)GameObjects.IconBackground), GetObject((int)GameObjects.Content).transform);
            slot.SetActive(true);
            Image image = Util.FindChild<Image>(slot, "CharacterIcon", false);
            image.sprite = Managers.Resource.Load<Sprite>("A10003");
        }
    }

    public void OnClicCharacterIamage()
    {

    }




}
