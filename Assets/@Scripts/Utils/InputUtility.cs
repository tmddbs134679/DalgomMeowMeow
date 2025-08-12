using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public static class InputUtility
{
    public static bool IsPointerOverUI(int fingerId = -1)
{
    if (EventSystem.current == null)
        return false;

#if UNITY_EDITOR || UNITY_STANDALONE
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    var results = new System.Collections.Generic.List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);
    return results.Count > 0;
#else
    return EventSystem.current.IsPointerOverGameObject(fingerId);
#endif
}
}
