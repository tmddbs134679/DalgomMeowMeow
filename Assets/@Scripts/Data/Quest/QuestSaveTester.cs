using UnityEngine;

public class QuestSaveTester : MonoBehaviour
{
    void Start()
    {
        var data = SaveQuestSystem.Load();
        QuestManager.Instance.LoadFromSaveData(data);
    }

    void OnApplicationQuit()
    {
        var data = QuestManager.Instance.GetAllQuestSaveData();
        SaveQuestSystem.Save(data);
    }
}