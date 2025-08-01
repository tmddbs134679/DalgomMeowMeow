using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageDataManager : MonoBehaviour
{
    public static StageDataManager Instance { get; private set; }
    public Character[] PlayerCharacter; // 플레이어 캐릭터
    public List<StageSO> stages;


    public int CurrentStageNumber;
    public int PendingGoldReward { get; private set; }
    public int PendingExpReward { get; private set; }

    private string SavePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            SavePath = Path.Combine(Application.persistentDataPath, "stage_save.json");
            LoadStage(); // 스테이지 데이터 로드
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
        SaveStage(); // 스테이지 데이터 저장
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

    IEnumerator HandleSceneReward()
    {
        yield return null; 
        yield return null; 

        var manager = StageDataManager.Instance;
        if (manager.PendingGoldReward > 0 || manager.PendingExpReward > 0)
        {
            Managers.Game.Gold += manager.PendingGoldReward;

            for (int k = 0; k < PlayerCharacter.Length; k++)
            {
                foreach (var character in Managers.Game.CharacterInMainScene)
                {
                    if (character.Value.Stat.data.UniqueId == PlayerCharacter[k].UniqueId)
                    {
                        character.Value.Stat.GainExp(manager.PendingExpReward);
                    }
                }
            }

            manager.ClearReward();
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(HandleSceneReward());
    }

    public void ClearReward()
    {
        PendingGoldReward = 0;
        PendingExpReward = 0;
    }


    public void SaveStage()
    {
        StageSaveData saveData = new StageSaveData
        {
            CurrentStageNumber = CurrentStageNumber
        };

        string json = JsonUtility.ToJson(saveData, true); // pretty print
        File.WriteAllText(SavePath, json);
    }

    public void LoadStage()
    {
        if(!File.Exists(SavePath))
        {
            CurrentStageNumber = 0; // 기본값
            return;
        }

        string json = File.ReadAllText(SavePath);
        StageSaveData saveData = JsonUtility.FromJson<StageSaveData>(json);
        CurrentStageNumber = saveData.CurrentStageNumber;
    }

    [ContextMenu("Delete Stage Save")]
    public void DeleteStageSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
    }
}


[System.Serializable]
public class StageSaveData
{
    public int CurrentStageNumber;
}