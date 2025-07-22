using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TeamCameraController _teamCameraController;
    [SerializeField] private TeamController _teamController;
    public int EnemyCount;
    public int PlayerCount;
    public bool Victory = false;
    public bool Lose = false;
    private int _enemyLayer;
    private int _playerLayer;

    private void Awake()
    {
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _playerLayer = LayerMask.NameToLayer("Player");
        _teamCameraController = GetComponentInChildren<TeamCameraController>();
        _teamController = GetComponentInChildren<TeamController>();

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
            _teamController._members.ForEach(m => { m.AttackRange = 1; m.Agent.stoppingDistance = 1; });
            _teamController._members.ForEach(m => m.CharacterObject.transform.localScale = new Vector3(1, 1, 1));
            _teamController._members.ForEach(m => m.victory = true); // 승리 머티리얼 적용
            _teamController._members.ForEach(m => m.SetOutline()); // 승리 머티리얼 적용


            _teamCameraController.Victory();

            Reward(); //보상 지급
            if (StageDataManager.Instance.CurrentStageNumber + 1< StageDataManager.Instance.stages.Count)
                StageDataManager.Instance.StageClear(); //스테이지 넘버 증가
            ForestBattleContext.IsVictory = true; //숲에 승리
        }

        if(PlayerCount == 0 && !Lose)
        {
            Lose = true;
            Time.timeScale = 0f; //게임 일시정지
            Managers.UI.ShowPopupUI<UI_Lose>();
        }
    }
    public void BackToGame()
    {
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }
    public void Reward()
    {
        StageDataManager.Instance.Reward(); //보상 지급
    }
    
}
