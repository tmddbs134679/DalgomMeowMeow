using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TravelBuilding : BuildingBase
{
    public override void OnClick()
    {
        List<Character> characters = Managers.Game.Characters;
        bool hasTravelModeCharacter = characters.Any(c => c.IsTravelMode);
        if (!hasTravelModeCharacter)
        {
            Managers.UI.ShowPopupUI<UI_TravelPopup>();
        }
        else
        {
            Debug.Log("여행중");
        }
 
    }

    public override void Produce()
    {
        
    }
}
