using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TeamCameraController _teamCameraController;
    [SerializeField] private GameObject _titleBtn;
    private StageManager _currentstage
        ;
    public int EnemyCount;
    public int PlayerCount;
    public bool Victory = false;

    private int _enemyLayer;
    private int _playerLayer;

    private void Awake()
    {
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _playerLayer = LayerMask.NameToLayer("Player");
        _teamCameraController = GetComponentInChildren<TeamCameraController>();
        _currentstage = GetComponentInParent<StageManager>();
    }
    private void Start()
    {
        BattleCharacter[] allCharacters = GetComponentsInChildren<BattleCharacter>();

        EnemyCount = 0;

        foreach (var character in allCharacters)
        {
            if (character.gameObject.layer == _enemyLayer)
                EnemyCount++;
        }

        PlayerCount = 0;

        foreach (var character in allCharacters)
        {
            if (character.gameObject.layer == _playerLayer)
                PlayerCount++;
        }
    }

    private void Update()
    {
        if (EnemyCount == 0 && !Victory)
        {
            Victory = true;
            _teamCameraController.Victory();
            Invoke(nameof(CallBtn), 2f);

            if (_currentstage.currentStageIndex >= Managers.Game.CurrentStage)
                Managers.Game.CurrentStage++;
        }

        if(PlayerCount == 0)
        {
            //게임오버 로직
        }
    }
    public void CallBtn()
    {
        _titleBtn.SetActive(true);
    }

    public void BackToGame()
    {
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }
}
