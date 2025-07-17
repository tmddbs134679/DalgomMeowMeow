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
                ch.CurrentState = Define.EAIState.Idle;
                ch.Pos = new Vector3Data(new Vector3(0f, 0.616f, 0f));
                AICharacter ai = Managers.Object.Spawn<AICharacter>(new Vector3(0f,0.616f,0f), ch.DataId);
                
                Managers.Game.CharacterInMainScene[ch.UniqueId] = ai;
                ai.Init();
                ai.SetInfo(ch);
                Managers.AI.ValidateNavMeshPosition(ai);
        }

    }


    public override void Clear()
    {
        Managers.Game.SaveGame();
    }
}
