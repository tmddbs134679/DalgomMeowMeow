using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_TravelCheckPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,

    }

    enum Buttons
    {
        BackgroundButton,
        RewardButton,
        CancelButton
    }

    enum Texts
    {
        TravelTimeText,
        TravelRemainText
    }

    enum Images
    {
        TravelImage
    }
    #endregion

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        StartCoroutine(StartTimer());

        Refresh();

    }



    private void OnDisable()
    {

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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgrondButton);

        GetButton((int)Buttons.RewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.RewardButton).gameObject.BindEvent(OnClickRewardButton);

        GetButton((int)Buttons.CancelButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton);
        return true;
    }

    private void OnClickRewardButton()
    {
        Managers.Game.OnTravelComplete();
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickBackgrondButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickCancelButton()
    {

        Managers.Game.ReturnFromTravel();

        Managers.UI.ClosePopupUI(this);
    }



    IEnumerator StartTimer()
    {
        while (Managers.Time.IsTraveling)
        {
            TimeSpan remaining = Managers.Time.TravelRemainingTime;

            //  UI 업데이트
             GetText((int)Texts.TravelRemainText).text = string.Format("{0:D2}:{1:D2}:{2:D2}", remaining.Hours, remaining.Minutes, remaining.Seconds);

            
            yield return new WaitForSeconds(1f);
        }

        GetButton((int)Buttons.RewardButton).gameObject.SetActive(true);
        GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);

    }


    private void Refresh()
    {
        TimeSpan duration = Managers.Time.TravelDuration;
        GetText((int)Texts.TravelTimeText).text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);
        //여행시간 다 지났을 경우
        if (Managers.Time.TravelRemainingTime <= TimeSpan.Zero)
        { 
            GetButton((int)Buttons.RewardButton).gameObject.SetActive(true);
            GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.RewardButton).gameObject.SetActive(false);
            GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
        }
    }

}
