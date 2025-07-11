using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageDataManager : MonoBehaviour
{
    public static StageDataManager Instance { get; private set; }

    public List<StageSO> stages;

    public int CurrentStageNumber;
    public int PendingGoldReward { get; private set; }
    public int PendingExpReward { get; private set; }

    public Character[] PlayerCharacter; // 플레이어 캐릭터
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Method to set the stage data, can be expanded as needed
    public StageSO SetStage()
    {
        StageSO found = stages.Find(x => x.StageNumber == CurrentStageNumber);
        return found != null ? found : null;
    }

    public void StageClear()
    {
        CurrentStageNumber += 1;
    }
    public void Reward()
    {
        StageSO stage = SetStage();
        if (stage != null)
        {
            PendingGoldReward = stage.GoldReward;
            PendingExpReward = stage.ExpReward;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var manager = StageDataManager.Instance;
        if (manager.PendingGoldReward > 0 || manager.PendingExpReward > 0)
        {
            Managers.Game.Gold += manager.PendingGoldReward;
            //Managers.Game.Exp += manager.PendingExpReward;
                Debug.Log($"🎉 보상 지급: Gold +{manager.PendingGoldReward}, Exp +{manager.PendingExpReward}");
                // 초기화
            manager.ClearReward();
        }
    }

    public void ClearReward()
    {
        PendingGoldReward = 0;
        PendingExpReward = 0;
    }
}
