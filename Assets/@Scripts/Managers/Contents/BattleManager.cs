using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private int _enemyCount;
    private bool _victory = false;

    private int enemyLayer;

    private void Awake()
    {
         enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    private void Start()
    {
        BattleCharacter[] allCharacters = GetComponentsInChildren<BattleCharacter>();

        _enemyCount = 0;
        foreach (var character in allCharacters)
        {
            if (character.gameObject.layer == enemyLayer)
                _enemyCount++;
        }

        Debug.Log($"적 유닛 수: {_enemyCount}");
    }

    private void Update()
    {
        if (_enemyCount == 0)
        {
            _victory = true;
            Debug.Log("모든 적을 처치했습니다! 승리!");
        }
    }
}
