using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckOutItem : UI_Base
{

    #region Enums
    enum GameObjects
    {
        DayValueObject
    }

    enum Texts
    {
        DayValueText,
        RewardValueText,
        ItemText
    }

    enum Images
    {

    }

    #endregion

    int _dayCount;
    bool _isCheckOut;
    bool _isVisul;
    private void OnEnable()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));



        #endregion
        
        gameObject.BindEvent(OnClickRewardConfirm);
        gameObject.AddComponent<UI_ButtonAnimation>();


        Refresh();
        return true;
    }

    private void OnClickRewardConfirm()
    {
        if (_isVisul)
            return;
        //보상획득
        //Managers.Game..~~


        UI_CheckOutRewardPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutRewardPopup>();
        popup.SetInfo(_dayCount);
    }

    public void SetInfo(int dayCount, bool isCheckOut, bool isVisul = false)
    {
        transform.localScale = Vector3.one;

        _dayCount = dayCount;
        _isCheckOut = isCheckOut;
        _isVisul = isVisul;

        if(_isVisul)
        {
            GetObject((int)GameObjects.DayValueObject).SetActive(false);

        }

        Refresh();
    }

    public void VisuwlObjectSetting()
    {
        _isVisul = true;
    }

    private void Refresh()
    {

        if (_init == false)
            return;

        if (_dayCount == 0)
            return;

        //int rewardMaterialId = Managers.Data.CheckOutDataDic[_dayCount].RewardItemId;
        int rewardItemValue = Managers.Data.CheckOutDataDic[_dayCount].RewardItemValue;

        GetText((int)Texts.DayValueText).text = _dayCount.ToString();
        GetText((int)Texts.RewardValueText).text = rewardItemValue.ToString();
    }
}
