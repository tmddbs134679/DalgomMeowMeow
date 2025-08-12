using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public static class InputUtility
{
    public static bool IsPointerOverUI()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        else
            return false;
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }
}
