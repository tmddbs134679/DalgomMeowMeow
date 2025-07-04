using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestProgressState { NotStarted, InProgress, Completed, Rewarded }

public class Quest
{
    public QuestDataSO QuestData;
    public int Progress;
    public QuestProgressState State;

    
    // 저장용
    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            QuestId = QuestData.QuestId,
            Progress = this.Progress,
            State = this.State
        };
    }

    // 불러오기용
    public void LoadProgress(QuestSaveData data)
    {
        this.Progress = data.Progress;
        this.State = data.State;
    }
    
    public Quest(QuestDataSO data)
    {
        QuestData = data;
        Progress = 0;
        State = string.IsNullOrEmpty(data.PreviousQuestId) ? QuestProgressState.InProgress : QuestProgressState.NotStarted;
    }

    public void AddProgress(int amount = 1)
    {
        if (State != QuestProgressState.InProgress) return;

        Progress += amount;
        if (Progress >= QuestData.GoalCount)
        {
            State = QuestProgressState.Completed;
            Managers.Debug.Log($"[퀘스트 완료] {QuestData.Title}", Define.EDebugType.Building);
        }
    }

    public void Reward()
    {
        if (State == QuestProgressState.Completed)
        {
            State = QuestProgressState.Rewarded;
            Managers.Debug.Log($"[보상 지급] {QuestData.Reward}", Define.EDebugType.Building);
            Managers.Game.Gold += QuestData.Reward.Gold;
            
            // 다음 퀘스트 열기
            QuestManager.Instance.TryActivateNext(QuestData.QuestId);
        }
    }
    
    public void SetProgress(int value)
    {
        Progress = value;
        if (Progress >= QuestData.GoalCount)
        {
            State = QuestProgressState.Completed;
            Debug.Log($"[퀘스트 자동완료] {QuestData.Title}");
        }
    }
}
