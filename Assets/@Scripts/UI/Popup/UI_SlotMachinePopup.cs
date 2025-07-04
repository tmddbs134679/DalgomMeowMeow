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
        Background
    }
    enum Texts { Slot1, Slot2, Slot3, Result }
    private SlotMachineBuilding _targetBuilding;
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SlotButton).gameObject.BindEvent(OnClickSlotButton);
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);

        return true;
    }

    private void OnClickBackgroundButton()
    {
        this.gameObject.SetActive(false);
    }

    private void OnClickSlotButton()
    {
        // 슬롯 실행 → 결과 받아오기
        string[] result;
        int reward;
        SlotMachineBuilding slot = FindObjectOfType<SlotMachineBuilding>();
        (result, reward) = slot.RollSlotAndReturn();
        
        GetText((int)Texts.Slot1).text = $"{result[0]}";
        GetText((int)Texts.Slot2).text = $"{result[1]}";
        GetText((int)Texts.Slot3).text = $"{result[2]}";
        GetText((int)Texts.Result).text = reward switch
        {
            > 0 => $"Congratulation: {reward} Gold",
            < 0 => $"Bad Shark ! {reward} Gold!",
            _ => "Try again!"
        };

    }
    
    public void SetTarget(SlotMachineBuilding building)
    {
        _targetBuilding = building;
    }
}
