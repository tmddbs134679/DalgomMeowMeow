using Data;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/StageSO", order = 1)]
public class StageSO : ScriptableObject
{
    public int StageNumber;
    public string[] EnemyID;
    public float[] EnemySpawnRate;
    public StageType StageType;

    public int ExpReward;
    public int GoldReward;

}
public enum StageType
{
    Normal,
    Boss,
    MiniBoss
}
