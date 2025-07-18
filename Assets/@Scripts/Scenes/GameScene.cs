using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (var ch in Managers.Game.Characters.Where(ch => ch.InMainScene))
        {
            Vector3 pos = ch.Pos.ToVector3() == Vector3.zero
            ? new Vector3(39f, 0.616f, 27f)
            : ch.Pos.ToVector3();
            AICharacter ai = Managers.Object.Spawn<AICharacter>(pos, ch.DataId);
            ai.Init();
            ai.SetInfo(ch);
            ch.InMainScene = true;

            Managers.Game.CharacterInMainScene[ch.UniqueId] = ai;
            Managers.Game.SetInitEquipment(Managers.Game.CharacterInMainScene[ch.UniqueId]);
            Managers.AI.ValidateNavMeshPosition(ai);
            Managers.AI.Register(ai);
        }

    }


    public override void Clear()
    {
        Managers.Game.SaveGame();
    }

}
