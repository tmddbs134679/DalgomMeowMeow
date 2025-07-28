using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ObjectManager 
{

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

    }

    public T Spawn<T>(Vector3 position, string templateID, Transform parent = null, bool isReplica = false, string prefabName = "") where T : BaseObject
    {
        System.Type type = typeof(T);
        GameObject go = null;

        // 1. 캐릭터 (AICharacter)
        if (type == typeof(AICharacter))
        {
            string prefabLabel = Managers.Data.CreatureDic[templateID].PrefabLabel;

            if (parent != null)
            {
                go = Managers.Resource.Instantiate(prefabLabel, parent, false);
                go.transform.localPosition = Vector3.zero;
            }
            else
            {
                go = Managers.Resource.Instantiate(prefabLabel, pooling: true);
                go.transform.position = position;
            }

            AICharacter ai = go.GetOrAddComponent<AICharacter>();


            if (isReplica)
            {
                ai.GetComponent<NavMeshAgent>().enabled = false;
                ai.IsReplica = true;
                Util.SetLayerRecursively(ai.gameObject, 30);
            }

            return ai as T;
        }

        // 2. 장비 (EquipmentController)
        else if (type == typeof(EquipmentController))
        {
            string prefabLabel = templateID;
            if (parent != null)
            {
                go = Managers.Resource.Instantiate(prefabLabel, parent, false);
            }
            else
            {
                go = Managers.Resource.Instantiate(prefabLabel, pooling: true);
                go.transform.position = position;
            }

        
            return go.GetOrAddComponent<EquipmentController>() as T;
        }

        else if (type == typeof(BattleCharacter))
        {
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

        }

        else if (type == typeof(Room))
        {
            string prefabLabel = templateID;
            if (parent != null)
            {
                go = Managers.Resource.Instantiate(prefabLabel, parent, false);
            }
            else
            {
                go = Managers.Resource.Instantiate(prefabLabel, pooling: true);
                go.transform.position = position;
            }


            return go.GetOrAddComponent<Room>() as T;
        }
        return null;
    }

}
