using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageDataManager : MonoBehaviour
{
    public static StageDataManager Instance { get; private set; }

    public List<StageSO> stages;

    public int CurrentStageNumber;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StageClear()
    {
        CurrentStageNumber += 1;
    }
    // Method to set the stage data, can be expanded as needed
    public StageSO SetStage()
    {
        StageSO found = stages.Find(x => x.StageNumber == CurrentStageNumber);
        return found != null ? found : null;
    }
}
