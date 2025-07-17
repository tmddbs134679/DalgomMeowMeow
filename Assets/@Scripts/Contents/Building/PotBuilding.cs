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
        if (Managers.Time.LastRewardTime > Managers.Time.LastLoginTime)
        {
            // 이미 보상 받음
            Managers.UI.ShowToast("이미 보상을 받았습니다!");
        }
        else
        {
            // 보상 받을 수 있음 
            Managers.UI.ShowPopupUI<UI_PotPopup>();
        }
    }
}
