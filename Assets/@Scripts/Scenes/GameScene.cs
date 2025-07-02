using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        SceneType = Define.EScene.GameScene;

        Managers.UI.ShowSceneUI<UI_GameScene>();
      //  Managers.UI.ShowPopupUI<UI_SaveMoveBuild>();

        foreach (var ch in Managers.Game.Characters)
        {
            AICharacter ai = Managers.Object.Spawn<AICharacter>(ch.Pos.ToVector3(), ch.DataId);
            ai.SetInfo(ch);
        }
    }


    public override void Clear()
    {
       Managers.Game.SaveGame();
    }

}
