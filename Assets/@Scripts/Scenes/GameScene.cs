using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

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

            Managers.Game.CharactersInScene[ch.UniqueId] = ai;
            Managers.Game.SetInitEquipment(Managers.Game.CharactersInScene[ch.UniqueId]);
            Managers.AI.ValidateNavMeshPosition(ai);
            Managers.AI.Register(ai);
        }

    }


    public override void Clear()
    {
       Managers.Game.SaveGame();
    }

}
