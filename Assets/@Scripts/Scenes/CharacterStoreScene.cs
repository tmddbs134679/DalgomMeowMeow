using Scripts.Contents.AI.FSM.State;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterStoreScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.EScene.CharacterStoreScene;

        Managers.UI.ShowSceneUI<UI_CharacterStoreScene>();
        Managers.Room.rooms.Clear();
        for(int i = 0; i < Managers.Room.UnLockRoom; i++)
        {
            Managers.Room.CreateRoom(Managers.Room.directions[i]);
        }

        foreach (var ch in Managers.Game.Characters.Where(c => !c.InMainScene))
        {
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 basePos = Vector3.zero;

            switch (ch.UniqueRoomNumber)
            {
                case 1: basePos = Vector3.zero; break;
                case 2: basePos = new Vector3(9.5f, 5f, 0f); break;
                case 3: basePos = new Vector3(0f, 5f, -9.5f); break;
                default:
                    Debug.LogWarning($"Unknown room number: {ch.UniqueRoomNumber}");
                    break;
            }
            Vector3 pos = basePos;
            pos.y += 0.5f; // or NavMesh 높이 보정

            var ai = Managers.Object.Spawn<AICharacter>(pos, ch.DataId);

            ai.Data = ch;
            ai.Init();
            ai.ControllerRegister();
            
            
        }

    }


    public override void Clear()
    {
        //Managers.Game.SaveGame();
    }
}
