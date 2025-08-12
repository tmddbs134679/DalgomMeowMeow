using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TravelBuilding : BuildingBase
{
    public override void OnClick()
    {
                if (InputUtility.IsPointerOverUI())
            return;
        List<Character> characters = Managers.Game.Characters;
        bool hasTravelModeCharacter = characters.Any(c => c.IsTravelMode);
        if (!hasTravelModeCharacter)
        {
            Managers.UI.ShowPopupUI<UI_TravelPopup>();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_TravelCheckPopup>();
        }
 
    }

    public override void Produce()
    {
        
    }
}
