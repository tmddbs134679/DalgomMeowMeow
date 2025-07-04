using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SlotMachinePopup : UI_Popup
{
    enum GameObjects
    {
        Content
    }

    enum Buttons
    {
        SlotButton,
    }
    enum Texts { Slot1, Slot2, Slot3 }
    private SlotMachineBuilding _targetBuilding;
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SlotButton).gameObject.BindEvent(OnClickSlotButton);

        return true;
    }

    private void OnClickSlotButton()
    {
        _targetBuilding.RollSlot();
    }
    
    public void SetTarget(SlotMachineBuilding building)
    {
        _targetBuilding = building;
    }
}
