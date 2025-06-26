using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    //int _order = 10;
    //int _toastOrder = 500;

    UI_Scene _sceneUI = null;
    public UI_Scene SceneUI { get { return _sceneUI; } }

    //public event Action<int> OnTimeScaleChanged;


    public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"{name}", parent, pooling);
        go.transform.SetParent(parent);
        return Util.GetOrAddComponent<T>(go);
    }
}
