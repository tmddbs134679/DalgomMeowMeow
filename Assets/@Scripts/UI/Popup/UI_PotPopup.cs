using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_PotPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject
    }

    enum Buttons
    {
        BackgroundButton,
        AdRewardButton,
        RewardButton
    }

    enum Texts
    {
        RewardValueText,

    }
    #endregion



    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetButton((int)Buttons.AdRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.RewardButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.AdRewardButton).gameObject.BindEvent(OnClickAdRewardButton);
        GetButton((int)Buttons.RewardButton).gameObject.BindEvent(OnClickRewardButton);

        Refresh();

        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this); 
    }

    private void OnClickAdRewardButton()
    {
        Managers.Ads.ShowRewardedAd(() =>
        {
            Managers.Time.GiveOfflineGold(true);
        });

    }

    private void OnClickRewardButton()
    {
        Managers.Time.GiveOfflineGold();

    }


    void Refresh()
    {
        TimeSpan offlineTime = DateTime.Now - Managers.Time.LastLoginTime;


        int totalMinutes = (int)offlineTime.TotalMinutes;
        int totalGold = totalMinutes * Define.GOLD_PER_MINUTE;

        GetText((int)Texts.RewardValueText).text = totalGold.ToString();

    }


}
