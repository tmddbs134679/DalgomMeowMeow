using UnityEngine;

public class QuestSaveTester : MonoBehaviour
{
    void Start()
    {
        // 게임 시작 시 저장된 퀘스트 데이터 로드
        var saveData = SaveQuestSystem.Load();
        QuestManager.Instance.LoadFromSaveData(saveData);
    }

    void OnApplicationQuit()
    {
        // 게임 종료 시 퀘스트 데이터 저장
        var data = QuestManager.Instance.GetAllQuestSaveData();
        SaveQuestSystem.Save(data);
        Managers.Game.SaveGame();
    }
}