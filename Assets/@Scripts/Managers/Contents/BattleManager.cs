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
    private BattleCharacter[] allCharacters;

    private void Awake()
    {
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _playerLayer = LayerMask.NameToLayer("Player");
        _teamCameraController = GetComponentInChildren<TeamCameraController>();
        _teamController = GetComponentInChildren<TeamController>();
    }

    private void Start()
    {
        allCharacters = GetComponentsInChildren<BattleCharacter>();
        
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
            _teamController._members.ForEach(m => m.victory = true); 
            _teamController._members.ForEach(m => m.SetOutline()); 

            _teamCameraController.Victory();

            Reward();
            if (StageDataManager.Instance.CurrentStageNumber + 1< StageDataManager.Instance.stages.Count)
                StageDataManager.Instance.StageClear(); 
            ForestBattleContext.IsVictory = true; 
        }

        if(PlayerCount == 0 && !Lose)
        {
            Lose = true;
            Time.timeScale = 0f; 
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
