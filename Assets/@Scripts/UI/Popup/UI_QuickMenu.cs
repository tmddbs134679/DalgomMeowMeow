using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuickMenu : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        CharacterNotifyObject,
        EquipmentNotifyObject,
        CheapterNotifyObject
    }

    enum Buttons
    {
        BackgroundButton,
        CharacterInfoButton,
        CharacterEquipmentButton,
        CharacterStoreSceneButton,
        ChapterButton,
        Stop_MinigameButton,
        Whack_a_moleButton,
    }

    enum Texts
    {

    }
    #endregion

    UI_CharacterPopup _characterPopupUI;
    UI_EquipPopup _EquipPopupUI;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        CheckNotify();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));


        _characterPopupUI = Managers.UI.ShowPopupUI<UI_CharacterPopup>();
        _EquipPopupUI = Managers.UI.ShowPopupUI<UI_EquipPopup>();

        _characterPopupUI.gameObject.SetActive(false);
        _EquipPopupUI.gameObject.SetActive(false);

        GetButton((int)Buttons.CharacterInfoButton).gameObject.BindEvent(OnClickCharacterInfoButton);
        GetButton((int)Buttons.CharacterInfoButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CharacterEquipmentButton).gameObject.BindEvent(OnClickCharacterEquipmentButton);
        GetButton((int)Buttons.CharacterEquipmentButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.CharacterStoreSceneButton).gameObject.BindEvent(OnClickCharacterStoreSceneButton);
        GetButton((int)Buttons.CharacterStoreSceneButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ChapterButton).gameObject.BindEvent(OnClickChapterButtonButton);
        GetButton((int)Buttons.Stop_MinigameButton).gameObject.BindEvent(OnClickStopminigameButton);
        GetButton((int)Buttons.Whack_a_moleButton).gameObject.BindEvent(OnClickWhack_a_moleButton);


        return true;
    }

    private void OnClickChapterButtonButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_ChapterPopup>();
        gameObject.SetActive(false);
    }


    private void OnClickCharacterEquipmentButton()
    {
        Managers.Sound.PlayButtonClick();
        _EquipPopupUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickCharacterInfoButton()
    {
        Managers.Sound.PlayButtonClick();
        _characterPopupUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickCharacterStoreSceneButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Scene.LoadScene(Define.EScene.CharacterStoreScene, transform);
    }


    private void OnClickBackgroundButton()
    {
        gameObject.SetActive(false);
    }

    private void OnClickStopminigameButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_StopMiniGame>();
    }

    private void OnClickWhack_a_moleButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_Whack_a_mole>();
    }

    private void CheckNotify()
    {
        //장비 
        if (Managers.Game.OwnedEquipments.Any(e => !e.IsConfirmed))
            GetObject((int)GameObjects.EquipmentNotifyObject).SetActive(true);
        else
            GetObject((int)GameObjects.EquipmentNotifyObject).SetActive(false);

        //캐릭터 
        if (Managers.Game.Characters.Any(c => !c.IsConfirmed))
            GetObject((int)GameObjects.CharacterNotifyObject).SetActive(true);
        else
            GetObject((int)GameObjects.CharacterNotifyObject).SetActive(false);

        if(QuestManager.Instance.CheapterNotify)
            GetObject((int)GameObjects.CheapterNotifyObject).SetActive(true);
        else
            GetObject((int)GameObjects.CheapterNotifyObject).SetActive(false);
    }
}
