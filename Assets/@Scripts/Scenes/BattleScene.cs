using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.EScene.BattleScene;
        Managers.Sound.Play(Define.ESound.Bgm, "TeampBattle");


    }


    public override void Clear()
    {
        Managers.Game.SaveGame();
    }
}
