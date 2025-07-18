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

        for(int i = 0; i < Managers.Room.UnLockRoom; i++)
        {
            Managers.Room.CreateRoom(Managers.Room.directions[i]);
        }

        foreach (var ch in Managers.Game.Characters.Where(c => !c.InMainScene))
        {
            var pos = new Vector3(Random.Range(-5f, 5f), 0.616f, Random.Range(-5f, 5f));
            var ai = Managers.Object.Spawn<AICharacter>(pos, ch.DataId);

            ai.Init();
            ai.Data = ch;
            ai.ControllerRegister();

            Managers.AI.ValidateNavMeshPosition(ai);
        }

    }


    public override void Clear()
    {
    }
}
