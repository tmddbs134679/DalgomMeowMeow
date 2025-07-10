using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gacha : UI_Popup
{
    enum GameObjects
    {

    }

    enum Buttons
    {
        BackgroundCloseButton,
        GachaButton,
    }
    enum Texts { }
    enum Images { }


    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        GetButton((int)Buttons.BackgroundCloseButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.GachaButton).gameObject.BindEvent(OnClickGachaButton);

        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = 38f;
        float z = 27f;
        return new Vector3(x, 0.616f, z);
    }
    private void OnClickGachaButton()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        Managers.Game.SpawnRandomGachaCharacter(spawnPos);

    }

}
