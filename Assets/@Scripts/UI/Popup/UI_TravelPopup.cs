using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_TravelPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        CharacterScrollContentObject
    }

    enum Buttons
    {
        BackgroundButton,
        RewardCheckButton,
        AdTravelButton,
        TravelButton,
    }

    enum Texts
    {
        TravelTimeValueText
    }

    enum Images
    {
        CharacterImage
    }
    #endregion

    ScrollRect _scrollrect;
    List<UI_TravelCharacterSlot> _slots = new List<UI_TravelCharacterSlot>();
    Character _character;

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        Managers.UI.OnTravelCharacter += SelectCharacter;
    }

    private void OnDisable()
    {
        Managers.UI.OnTravelCharacter -= SelectCharacter;
    }

    private void SelectCharacter(UI_TravelCharacterSlot slot)
    {
        OnSlotClicked(slot);
        _character = slot._character;

        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(slot._character.Data.IconLabel);
    }

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
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.RewardCheckButton).gameObject.BindEvent(OnClickRewardCheckButton);
        GetButton((int)Buttons.AdTravelButton).gameObject.BindEvent(OnClickAdTravelButton);
        GetButton((int)Buttons.TravelButton).gameObject.BindEvent(OnClickTravelButton);

        GetButton((int)Buttons.AdTravelButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.TravelButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();

        return true;
    }

    private void OnClickBackgroundButton()
    {
        _character = null;

        Managers.UI.ClosePopupUI(this); 
    }

    private void OnClickTravelButton()
    {
        if(_character == null)
        {
            Managers.UI.ShowToast("캐릭터를 먼저 선택하세요!");
            return;
        }
        Managers.Time.StartTravel(_character);
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickRewardCheckButton()
    {

    }

    private void OnClickAdTravelButton()
    {
        if (_character == null)
        {
            Managers.UI.ShowToast("캐릭터를 먼저 선택하세요!");
            return;
        }

        Managers.Ads.ShowRewardedAd(() =>
        {
            //여행시간 30분 줄이기.
            _character.IsTravelMode = true;
            Managers.UI.ClosePopupUI(this);
        });

    }

    public void Refresh()
    {

        _character = null;

        GetObject((int)GameObjects.CharacterScrollContentObject).DestroyChilds();

        List<Character> characters = Managers.Game.Characters;
        foreach (Character ch in characters)
        {
            UI_TravelCharacterSlot slot = Managers.UI.MakeSubItem<UI_TravelCharacterSlot>(GetObject((int)GameObjects.CharacterScrollContentObject).transform);
            slot.SetInfo(ch, _scrollrect);
            _slots.Add(slot);

        }
    }



    public void OnSlotClicked(UI_TravelCharacterSlot clickedSlot)
    {
        //  모든 슬롯 아웃라인 끄기
        foreach (var slot in _slots)
        {
            slot.SetOutline(false);
        }

        //  클릭된 슬롯만 아웃라인 켜기
        clickedSlot.SetOutline(true);
    }
}
