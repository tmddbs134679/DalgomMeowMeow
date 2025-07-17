using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Whack_a_mole : UI_Popup
{
    enum GameObjects
    {
        Mole_Manager,
    }


    enum Buttons
    {
        Start
    }

    public Button Startbtn;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        Startbtn = GetButton((int)Buttons.Start);
        Startbtn.gameObject.BindEvent(StartGame);

        Startbtn.gameObject.SetActive(true);

        return true;
    }


    private void StartGame()
    {
        GetObject((int)GameObjects.Mole_Manager).GetComponent<MoleManager>().StartGame();
        Startbtn.gameObject.SetActive(false);
    }

    
    public void PopupClose()
    {
        Managers.Debug.Log($"Closed", Define.EDebugType.None);
        Managers.UI.ClosePopupUI(this);
    }
}
