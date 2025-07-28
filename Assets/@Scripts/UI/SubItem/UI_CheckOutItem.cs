using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CheckOutItem : UI_Base
{

    #region Enums
    enum GameObjects
    {
        DayValueObject,
        ClearRewardCompleteObject
    }

    enum Texts
    {
        DayValueText,
        RewardValueText,
        ItemText
    }

    enum Images
    {
        RewardItemImage,
    }

    #endregion

    int _dayCount;
    bool _isCheckOut;
    bool _isVisul;
    private bool _isCanClick;
    int _itemID;
    int _rewardValue;
    //bool _isCanReward;

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
        GetObject((int)GameObjects.ClearRewardCompleteObject).SetActive(false);
        Refresh();
        return true;
    }

    private void OnClickRewardConfirm()
    {
        if (!_isCanClick)
            return;


        if (Managers.Game.AttendanceReceived[_dayCount - 1] == true)
        {
            Managers.Debug.Log("이미 보상을 받았습니다.", Define.EDebugType.UI);
            return;
        }

        Managers.Sound.Play(Define.ESound.Effect, "Money");

        Managers.Game.AttendanceReceived[_dayCount - 1] = true;

        //재화 지급
        Managers.Game.RewardMaterial(_itemID, _rewardValue);

        UI_CheckOutRewardPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutRewardPopup>();
        popup.SetInfo(_dayCount);

     
    }

    public void SetInfo(int dayCount, bool isCheckOut, bool canClick, bool isVisul = false)
    {
        transform.localScale = Vector3.one;

        _dayCount = dayCount;
        _isCheckOut = isCheckOut;
        _isVisul = isVisul;
        _isCanClick = canClick;


        _itemID = Managers.Data.CheckOutDataDic[_dayCount].RewardItemId;

        if (_isVisul)
        {
            GetObject((int)GameObjects.DayValueObject).SetActive(false);

        }

        if(_isCheckOut)
        {
            
            GetObject((int)GameObjects.ClearRewardCompleteObject).SetActive(true);
        }
        else if(_isCanClick)
        {
            gameObject.GetOrAddComponent<UI_ButtonAnimation>();
            GetObject((int)GameObjects.ClearRewardCompleteObject).SetActive(false);
        }
        else
        {
            GetObject((int)GameObjects.ClearRewardCompleteObject).SetActive(false);
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


        _rewardValue = Managers.Data.CheckOutDataDic[_dayCount].RewardItemValue;

        GetText((int)Texts.DayValueText).text = _dayCount.ToString();
        GetText((int)Texts.RewardValueText).text = _rewardValue.ToString();
        GetImage((int)Images.RewardItemImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_itemID].SpriteName);
    }
}
