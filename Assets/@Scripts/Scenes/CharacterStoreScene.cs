using Scripts.Contents.AI.FSM.State;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStoreScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.EScene.CharacterStoreScene;


        Managers.Room.rooms.Clear();
        for(int i = 0; i < Managers.Room.UnLockRoom; i++)
        {
            Managers.Room.CreateRoom(Managers.Room.directions[i]);
        }

        foreach (var ch in Managers.Game.Characters.Where(c => !c.InMainScene))
        {
            Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));

            var ai = Managers.Object.Spawn<AICharacter>(Vector3.zero, ch.DataId);

            ai.Stat.data = ch;
            ai.Init();
            ai.SetInfo(ch);
            ai.transform.position = ch.RoomPos.ToVector3();

            Managers.Game.CharacterInMainScene[ch.UniqueId] = ai;
            Managers.Equipment.SetInitEquipment(Managers.Game.CharacterInMainScene[ch.UniqueId]);

        }

        Managers.UI.ShowSceneUI<UI_CharacterStoreScene>();
    }


    public override void Clear()
    {
        Managers.Game.SaveGame();
    }
}
