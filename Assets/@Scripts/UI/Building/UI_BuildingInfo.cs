using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingInfo : UI_Base
{
    enum GameObjects
    {
        RootPanel,
    }

    enum TMP_Texts
    {
        BuildingNameText,
        StateText,
    }

    enum Buttons
    {
        CloseButton,
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(TMP_Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.CloseButton).gameObject, () =>
        {
            Close();
        });

        return true;
    }

    public void Open(BuildingBase building)
    {
        GetObject((int)GameObjects.RootPanel).SetActive(true);
        GetText((int)TMP_Texts.BuildingNameText).text = building.BuildingData.BuildingName;
        GetText((int)TMP_Texts.StateText).text = building.CurrentState.ToString();
    }

    public void Close()
    {
        GetObject((int)GameObjects.RootPanel).SetActive(false);
    }
}

