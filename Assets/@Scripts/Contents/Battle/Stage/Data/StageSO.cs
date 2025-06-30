using Data;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/StageSO", order = 1)]
public class StageSO : ScriptableObject
{
    public int StageNumber;
    [SerializeField]public CreatureData[] enemydata; //적 데이터들
    public float[] EnemySpawnRate;
    public StageType StageType;

}
public enum StageType
{
    Normal,
    Boss,
    MiniBoss
}
