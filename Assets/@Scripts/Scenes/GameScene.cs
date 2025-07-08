using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        SceneType = Define.EScene.GameScene;

        Managers.UI.ShowSceneUI<UI_GameScene>();    
        foreach (var ch in Managers.Game.Characters)
        {
            AICharacter ai = Managers.Object.Spawn<AICharacter>(ch.Pos.ToVector3(), ch.DataId);
            ai.Init();
            ai.SetInfo(ch);

            Managers.Game.CharactersInScene[ch.DataId] = ai;
            Managers.Game.SetInitEquipment(ai);
            Managers.AI.Register(ai);
        }

    }


    public override void Clear()
    {
       Managers.Game.SaveGame();
    }

}
