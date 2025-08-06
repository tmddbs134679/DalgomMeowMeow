using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_CheckOutPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        CheckOutBoardObject,
    }

    enum Buttons
    {
        ExitButton,
    }

    enum Texts
    {

    }
    #endregion


    public int _CheckOutDay;
    int _monthCount;
    int _dailyCount;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

    }


    private void OnDestroy()
    {
        Managers.Game.OnResourcesChagned -= Refresh;
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        #endregion

        Managers.Game.OnResourcesChagned += Refresh;
        Refresh();

        return true;
    }

    public void SetInfo(int checkOutDay)
    {
        _CheckOutDay = checkOutDay;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_CheckOutDay == 0)
            return;

        _monthCount = _CheckOutDay % 30;
        _dailyCount = _monthCount % 10;


        if (_dailyCount == 0)
        {
            _dailyCount = 10;
        }

        GetObject((int)GameObjects.CheckOutBoardObject).DestroyChilds();

        Transform parent = GetObject((int)GameObjects.CheckOutBoardObject).transform;

        int boardOffset = (_CheckOutDay / 10) * 10;
        for (int count = 1; count <= 10; count++)
        {
            // 전역 출석일수 (1일부터 시작)
            int globalDay = boardOffset + count;

            // 배열 범위 체크
            if (globalDay - 1 >= Managers.Game.AttendanceReceived.Length)
                break;

            bool isReceived = Managers.Game.AttendanceReceived[globalDay - 1];

            UI_CheckOutItem item = Managers.UI.MakeSubItem<UI_CheckOutItem>(parent);
            item.transform.SetAsLastSibling();

            if (_CheckOutDay >= globalDay)
            {
                // 출석일이 지났음 → 클릭 가능 여부
                item.SetInfo(globalDay, isReceived, canClick: !isReceived);
            }
            else
            {
                // 출석일이 아직 안 됨 → 잠김
                item.SetInfo(globalDay, isReceived, canClick: false);
            }

        }

    }

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
