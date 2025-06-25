using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        StorageObject
    }

    enum Buttons
    {
        MailButton,
        NoticeButton,
        SettingButton,
        QuestButton,
        ArchivementButton,
        DailyButton,
        ShopButton,
        BuildButton,
        EditPosButton,

    }

    enum Texts
    {
        PlayerGoldText,
        CreatureCountText,

    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));


        return true;
    }
    private void Awake()
    {
        Init();
    }






}
