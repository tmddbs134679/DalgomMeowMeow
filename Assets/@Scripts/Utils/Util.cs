using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 자주쓰이는 범용적인 함수들 
 */

public static class Util 
{

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }


    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    #region EventBus
    public class AnimalArrivedAtBuildingEvent : BaseEvent
    {
        public AICharacter Animal;
        public BuildingBase TargetBuilding;
    }

    public class BuildingProductionFinishedEvent : BaseEvent
    {
        public BuildingBase Building;
        public AICharacter AssignedAnimal;
    }

    #endregion

    public static AICharacter FindAIById(string dataId)
    {
        foreach (var ai in GameObject.FindObjectsOfType<AICharacter>())
        {
            if (ai.Data?.DataId == dataId)
                return ai;
        }
        return null;
    }

    public static int GetIndexInLinkedList<T>(LinkedList<T> list, T value)
    {
        int index = 0;
        foreach (var item in list)
        {
            if (EqualityComparer<T>.Default.Equals(item, value))
                return index;

            index++;
        }
        return -1; // 못 찾았을 때
    }

    public static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

}
