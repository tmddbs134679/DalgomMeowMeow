using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotBuilding : BuildingBase
{

    public override void Produce()
    {
        
    }
    

    public override void OnClick()
    {
        TimeSpan offlineTime = DateTime.Now - Managers.Time.LastRewardTime;

        if (offlineTime.TotalMinutes < 1)
        {
            //  최근에 이미 보상 받음
            Managers.UI.ShowToast("이미 보상을 받았습니다!");
        }
        else
        {
            //  보상 받을 수 있음 (팝업 열기)
            Managers.UI.ShowPopupUI<UI_PotPopup>();
        }
    }
}
