using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MiniGame : UI_Popup
{
    private Stop_MiniGameManager _gameManager;
    enum GameObjects
    {
        Player,
        Standing,
    }

    enum Buttons
    {
        ScreenTouch,
    }

    public bool LookBack = false;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ScreenTouch).gameObject.BindEvent(OnTouchScreen);

        _gameManager = GetObject((int)GameObjects.Standing).GetComponent<Stop_MiniGameManager>();



        return true;
    }

    public void OnTouchScreen()
    {
        if (_gameManager.IsLookBack)
        {
            //실패
            return;
        }
        GetObject((int)GameObjects.Player).transform.position += Vector3.right*10;
    }

}
