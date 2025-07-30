using UnityEngine;

public class GameScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        SceneType = Define.EScene.GameScene;

        Managers.UI.ShowSceneUI<UI_GameScene>();
        foreach (var ch in Managers.Game.Characters)
        {
                        //여행중이면 Pass
      

            if (!ch.InMainScene)
                continue;

            Vector3 pos = ch.Pos.ToVector3() == Vector3.zero
            ? new Vector3(39f, 0.616f, 27f)
            : ch.Pos.ToVector3();
            AICharacter ai = Managers.Object.Spawn<AICharacter>(pos, ch.DataId);
            ai.Init();
            ai.SetInfo(ch);
            ch.InMainScene = true;

            Managers.Game.CharacterInMainScene[ch.UniqueId] = ai;
            Managers.Equipment.SetInitEquipment(Managers.Game.CharacterInMainScene[ch.UniqueId]);
            Managers.AI.ValidateNavMeshPosition(ai);
            Managers.AI.Register(ai);

            if(Managers.Game.RewardMinigame)
            {
                Managers.Game.RewardMinigame = false;
                Managers.Game.DailyMiniGameReward();
            }

            if (ch.IsTravelMode)
            {
                ai.gameObject.SetActive(false);
            }
                
        }
            Managers.Sound.Play(Define.ESound.Bgm, "BGM1");
    }


    public override void Clear()
    {
        Managers.Game.SaveGame();
    }

}
