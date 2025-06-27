using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TeamCameraController _teamCameraController;
    [SerializeField] private GameObject _titleBtn;
    public int EnemyCount;
    public bool Victory = false;

    private int enemyLayer;

    private void Awake()
    {
         enemyLayer = LayerMask.NameToLayer("Enemy");
        _teamCameraController = GetComponentInChildren<TeamCameraController>();
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
        if (EnemyCount == 0 && !Victory)
        {
            Victory = true;
            _teamCameraController.Victory();
            _titleBtn.SetActive(true);
            if (!Managers.Game.CurrentStageCleared)
                Managers.Game.CurrentStage++;
        }
    }
}
