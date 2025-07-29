using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StopMiniGame : UI_Popup
{
    #region enums
    enum GameObjects
    {
        Player,
        Standing,
        Reward,
        Timer,
        HighScore,
    }
    

    enum Buttons
    {
        ScreenTouch,
        Start,
        Title,
    }
    #endregion

    private Stop_MiniGameManager _gameManager;
    private GameObject _player;

    public bool LookBack = false;
    public float Time = 60;
    public float Lefttime;
    public float FastestTime;

    public Button TitleButton;
    public Button StartButton;

    public GameObject RewardUI;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI HighScoreTxt;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        StartButton = GetButton((int)Buttons.Start);
        TitleButton = GetButton((int)Buttons.Title);
        _gameManager = GetObject((int)GameObjects.Standing).GetComponent<Stop_MiniGameManager>();
        _player = GetObject((int)GameObjects.Player);
        Timer = GetObject((int)GameObjects.Timer).GetComponent<TextMeshProUGUI>();
        HighScoreTxt = GetObject((int)GameObjects.HighScore).GetComponent<TextMeshProUGUI>();
        RewardUI = GetObject((int)GameObjects.Reward);

        StartButton.gameObject.BindEvent(StartGame);
        TitleButton.gameObject.BindEvent(PopupClose);

        RewardUI.SetActive(false);

        FastestTime = PlayerPrefs.GetFloat("FastestTime", 60f); // 플레이어의 가장 빠른 시간 불러오기

        CharacterSet();
        return true;
    }

    private void FixedUpdate()
    {
        if (_gameManager.IsGameOver)
        {
            return;
        }

        Time -= UnityEngine.Time.deltaTime; // 타이머 감소
        Timer.text = Time.ToString("F0"); // 소수점 첫째 자리까지 표시
        if (Time <= 0)
        {
            _gameManager.GameOver();
            GameEndUI();
        }
    }

    public void StartGame()
    {
        _gameManager.IsGameOver = false;
        Managers.Sound.PlayButtonClick();
        GetButton((int)Buttons.ScreenTouch).gameObject.BindEvent(OnTouchScreen);
        _gameManager.GameStart();
        StartButton.gameObject.SetActive(false);
    }

    public void OnTouchScreen()
    {
        if (_gameManager.IsGameOver)
        {
            return;
        }

        Managers.Sound.Play(Define.ESound.Effect, "Click4");


        if (_gameManager.IsLookBack)
        {
            _gameManager.GameOver();

            _player.transform.localScale = new Vector3(1.5f, 1, 1);
            Lefttime = 0f;
            GameEndUI(); // 게임 오버시 보상 UI 띄우기
            return;
        }

        if (_player.transform.localPosition.x > 830)
        {
            Lefttime = 60f - Time; // 남은 시간 저장
            if(Lefttime < FastestTime)
            {
                FastestTime = Lefttime; // 가장 빠른 시간 갱신
                PlayerPrefs.SetFloat("FastestTime", FastestTime); // 플레이어의 가장 빠른 시간 저장
            }
            _gameManager.GameClear();
            GameEndUI();
        }

        _player.transform.localPosition += Vector3.right * 10f;

        _player.transform.DOKill();
        _player.transform.localScale = Vector3.one;

        _player.transform.DOScale(1.1f, 0.1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _player.transform.DOScale(1f, 0.1f).SetEase(Ease.InBack);
            });
    }

    public void CharacterSet()
    {
        _player.transform.localPosition = new Vector3(-830, -150, 0);
        _gameManager.transform.localScale = Vector3.one; // 좌우 반전 초기화
    }

    public void GameEndUI()  //타이틀버튼 키고 UI
    {
        HighScoreTxt.text = $"현재 기록: {Lefttime:F2}s\n최단 기록: {FastestTime:F2}s";
        RewardUI.SetActive(true);
        int money =(int)Lefttime*100;
        RewardUI.GetComponentInChildren<TextMeshProUGUI>().text = "Reward\n" + money + "골드";

        //돈 입금
    }

    public void PopupClose()
    {
        Managers.Debug.Log($"Closed", Define.EDebugType.None);
        Managers.Sound.PlayPopupClose();
        Managers.UI.ClosePopupUI(this);
    }
}
