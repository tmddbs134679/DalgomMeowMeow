using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 *  기존 엔진 함수들을 확장해서 기능을 추가 하고 싶을 때 사용
 */


public static class Extension
{
    public static void BindEvent(this GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_Base.BindEvent(go, action, dragAction, type);
    }
}
