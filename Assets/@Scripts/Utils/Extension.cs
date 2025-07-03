using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Util;

/*
 *  기존 엔진 함수들을 확장해서 기능을 추가 하고 싶을 때 사용
 */


public static class Extension
{
    public static void BindEvent(this GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_Base.BindEvent(go, action, dragAction, type);
    }


    public static Vector3Data ToData(this Vector3 v) => new Vector3Data(v);
    public static Vector3 ToVector3(this Vector3Data v) => new Vector3(v.x, v.y, v.z);

    public static void DestroyChilds(this GameObject go)
    {
        Transform[] children = new Transform[go.transform.childCount];
        for (int i = 0; i < go.transform.childCount; i++)
        {
            children[i] = go.transform.GetChild(i);
        }

        // 모든 자식 오브젝트 삭제
        foreach (Transform child in children)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
    }

}
