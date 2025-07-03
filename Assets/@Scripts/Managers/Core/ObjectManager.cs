using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectManager 
{
    public HashSet<AICharacter> Characters { get; } = new HashSet<AICharacter>();

    public HashSet<BuildingBase> Buildings { get; } = new HashSet<BuildingBase>();

    public Transform CharacterTransform
    {
        get
        {
            GameObject root = GameObject.Find("Character");
            if (root == null)
                root = new GameObject { name = "Character" };
            return root.transform;
        }
    }

    public Transform BuildingTransform
    {
        get
        {
            GameObject root = GameObject.Find("Building");
            if (root == null)
                root = new GameObject { name = "Building" };
            return root.transform;
        }
    }

    public ObjectManager()
    {
        Init();
    }

    private void Init()
    {
       
    }

    public void Clear()
    {
        Characters.Clear();
        Buildings.Clear();
    }

    public T Spawn<T>(Vector3 position, string templateID, string prefabName = "") where T : BaseObject
    {
        System.Type type = typeof(T);

        if (type == typeof(AICharacter))
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel, pooling: true);
            go.transform.position = position;
            AICharacter pc = go.GetOrAddComponent<AICharacter>();
            return pc as T;
        }
        


            return null;
    }

    public T Spawn<T>(Vector3 position, string templateID, Transform parent) where T : BaseObject
    {
        System.Type type = typeof(T);
        GameObject go;

        // 부모가 있으면 parent 기준으로 생성
        if (parent != null)
        {
            go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel, parent, false);
            go.transform.localPosition = Vector3.zero;
        }
        else
        {
            go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel);
            go.transform.position = position;
        }

        if (type == typeof(AICharacter))
            return go.GetOrAddComponent<AICharacter>() as T;

        return null;
    }

}
