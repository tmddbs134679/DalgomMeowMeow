using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStoreScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.EScene.CharacterStoreScene;

        //Managers.UI.ShowSceneUI<UI_CharacterStoreScene>();
        foreach (var ch in Managers.Game.Characters)
        {
            if (!ch.InMainScene)
            {
                AICharacter ai = Managers.Object.Spawn<AICharacter>(Vector3.zero, ch.DataId);
                ai.Init();
                ai.ControllerRegister();

                Managers.Game.CharacterInMainScene[ch.UniqueId] = ai;
                Managers.Game.SetInitEquipment(Managers.Game.CharacterInMainScene[ch.UniqueId]);
                Managers.AI.ValidateNavMeshPosition(ai);
            }
        }

    }


    public override void Clear()
    {

    }
}
