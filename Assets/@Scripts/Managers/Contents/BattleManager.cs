using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int EnemyCount;
    public bool Victory = false;

    private int enemyLayer;

    private void Awake()
    {
         enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    private void Start()
    {
        BattleCharacter[] allCharacters = GetComponentsInChildren<BattleCharacter>();

        EnemyCount = 0;
        foreach (var character in allCharacters)
        {
            if (character.gameObject.layer == enemyLayer)
                EnemyCount++;
        }

        Debug.Log($"적 유닛 수: {EnemyCount}");
    }

    private void Update()
    {
        if (EnemyCount == 0)
        {
            Victory = true;
            Debug.Log("모든 적을 처치했습니다! 승리!");
        }
    }
}
