using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleScene : UI_Scene
{
    #region Enum

    enum Buttons
    {
        PauseButton,
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);



        return true;
    }
    UI_PausePopup _pausePopup;
    public void OnClickPauseButton()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        _pausePopup = Managers.UI.ShowPopupUI<UI_PausePopup>(); // PausePopup UI 표시
        _pausePopup.gameObject.SetActive(true);
    }
}
