using UnityEngine;

public enum QuestType { Main, Side, Daily }
public enum QuestConditionType { Collect, Kill, Talk, AssignAnimal }
public enum TargetType { Cat, Slime, Soup }

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public string QuestId;
    public string Title;
    public string Description;
    public QuestType Type;
    public QuestConditionType Condition;
    public TargetType TargetType; // 예: "Cat", "Soup"
    public int GoalCount;
    public RewardData Reward;
    public string PreviousQuestId;
}
