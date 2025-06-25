using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestProgressState { NotStarted, InProgress, Completed, Rewarded }

public class Quest
{
    public QuestDataSO QuestData;
    public int Progress;
    public QuestProgressState State;

    public Quest(QuestDataSO data)
    {
        QuestData = data;
        Progress = 0;
        State = QuestProgressState.InProgress;
    }

    public void AddProgress(int amount = 1)
    {
        if (State != QuestProgressState.InProgress) return;

        Progress += amount;
        if (Progress >= QuestData.GoalCount)
        {
            State = QuestProgressState.Completed;
            Debug.Log($"[퀘스트 완료] {QuestData.Title}");
        }
    }

    public void Reward()
    {
        if (State == QuestProgressState.Completed)
        {
            State = QuestProgressState.Rewarded;
            Debug.Log($"[보상 지급] {QuestData.Reward}");
        }
    }
}
