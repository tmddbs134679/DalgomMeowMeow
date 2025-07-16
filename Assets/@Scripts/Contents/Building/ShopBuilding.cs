using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilding : BuildingBase
{
    public override void Produce()
    {
        
    }

    public override void OnClick()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
         Managers.UI.ShowPopupUI<UI_ShopPopup>();
    }
}
