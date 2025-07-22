using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Define;


public class UI_CharacterStoreScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        QuickNotifyObject,
        CheckImage
    }

    enum Buttons
    {
        NoticeButton,
        SettingButton,
        QuestButton,
        CheckOutButton,
        QuickButton,
        ShopButton,
        HomeButton,
        ChangeButton,
        UnLockButton,
        Yes,
        No,
        RoomReset,
    }

    enum Texts
    {
        PlayerGoldText,
        CreatureCountText,
        DiaText,
    }

    enum Images
    {

    }

    #endregion

    UI_QuickMenu _quickMenuPopupUI;
    UI_CheckOutPopup _checkOutPopupUI;
    UI_ShopPopup _shopPopupUI;
    UI_EditSettingPopup _editSettingPopupUI;
    UI_ChangePopup _ChangePopupUI;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        _quickMenuPopupUI = Managers.UI.ShowPopupUI<UI_QuickMenu>();
        _checkOutPopupUI = Managers.UI.ShowPopupUI<UI_CheckOutPopup>();
        _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
        _editSettingPopupUI = Managers.UI.ShowPopupUI<UI_EditSettingPopup>();
        _ChangePopupUI = Managers.UI.ShowPopupUI<UI_ChangePopup>();

        _quickMenuPopupUI.gameObject.SetActive(false);
        _checkOutPopupUI.gameObject.SetActive(false);
        _shopPopupUI.gameObject.SetActive(false);
        _editSettingPopupUI.gameObject.SetActive(false);
        _ChangePopupUI.gameObject.SetActive(false);
        GetObject((int)GameObjects.CheckImage).gameObject.SetActive(false);

        GetButton((int)Buttons.QuickButton).gameObject.BindEvent(OnClickQuickButton);
        GetButton((int)Buttons.QuickButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CheckOutButton).gameObject.BindEvent(OnClickCheckOutButton);
        GetButton((int)Buttons.CheckOutButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuestButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuestButton).gameObject.BindEvent(OnClickQuestButton);

        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickEditSettingButton);
        GetButton((int)Buttons.SettingButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(OnClickShopButton);
        GetButton((int)Buttons.ShopButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);

        GetButton((int)Buttons.ChangeButton).gameObject.BindEvent(OnClickChangeButton);

        GetButton((int)Buttons.UnLockButton).gameObject.BindEvent(OnClickUnLockButton);
        GetButton((int)Buttons.Yes).gameObject.BindEvent(OnClickYes);
        GetButton((int)Buttons.No).gameObject.BindEvent(OnClickNo);
        GetButton((int)Buttons.RoomReset).gameObject.BindEvent(OnClickRoomReset);


        #region Action 추가
        Managers.Game.OnResourcesChagned += Refresh;
        Managers.Game.OnCharacterChanged += Refresh;


        #endregion

        Refresh();

        return true;
    }

    private void OnClickEditSettingButton()
    {
        _editSettingPopupUI.gameObject.SetActive(true);
    }

    private void OnClickShopButton()
    {
        _shopPopupUI.gameObject.SetActive(true);
    }

    private void OnClickChangeButton()
    {
        _ChangePopupUI.gameObject.SetActive(true);
    }

    private void OnClickUnLockButton()
    {
        if (Managers.Room.UnLockRoom >= 3)
        {
            Managers.UI.ShowToast("모든 방이 열렸습니다!");
            GetObject((int)GameObjects.CheckImage).gameObject.SetActive(false);
            return;
        }
        GetObject((int)GameObjects.CheckImage).gameObject.SetActive(true);
    }


    private void OnClickNo()
    {
        GetObject((int)GameObjects.CheckImage).gameObject.SetActive(false);
    }


    private void OnClickYes()
    {

        if (Managers.Game.Dia >= 300)
        {
            Managers.Room.UnLockRoom++;
            Managers.UI.ShowToast($"방이 총 {Managers.Room.UnLockRoom}개 열렸습니다!");
            for (int i = 0; i < Managers.Room.UnLockRoom; i++)
            {
                Managers.Room.CreateRoom(Managers.Room.directions[i]);
            }
            GetObject((int)GameObjects.CheckImage).gameObject.SetActive(false);
        }

        else
        {
            Managers.UI.ShowToast("다이아가 부족합니다!");
        }


    }

    private void OnClickRoomReset()
    {
        PlayerPrefs.DeleteKey("UnLockRoom");
    }


    private void OnClickCheckOutButton()
    {
        if (_checkOutPopupUI == null)
        {
            Managers.Debug.LogError("_checkOutPopupUI가 없음", EDebugType.UI);
            return;
        }

        _checkOutPopupUI.SetInfo(Managers.Time.AttendanceDay);
        _checkOutPopupUI.gameObject.SetActive(true);

    }

    private void OnClickHomeButton()
    {
        Managers.Scene.LoadScene(Define.EScene.GameScene, transform);
    }

    private void OnClickQuickButton()
    {
        _quickMenuPopupUI.gameObject.SetActive(true);
    }

    private void Awake()
    {
        Init();
    }




    void Refresh()
    {
        GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
        GetText((int)Texts.CreatureCountText).text = Managers.Game._characters.Count.ToString();
        GetText((int)Texts.DiaText).text = Managers.Game.Dia.ToString();

        CheckNotify();
    }



    #region Battle


    #endregion

    #region Quest
    private void OnClickQuestButton()
    {
        Managers.UI.ShowPopupUI<UI_QuestPopup>();
    }
    #endregion


    public void CheckNotify()
    {
        //장비, 캐릭터 업데이트된게 있으면 True
        if (Managers.Game.OwnedEquipments.Any(e => !e.IsConfirmed) || Managers.Game._characters.Any(c => !c.Value.IsConfirmed))
            GetObject((int)GameObjects.QuickNotifyObject).SetActive(true);
        else
            GetObject((int)GameObjects.QuickNotifyObject).SetActive(false);
    }
}
