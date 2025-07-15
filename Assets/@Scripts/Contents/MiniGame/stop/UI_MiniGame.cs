using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniGame : UI_Popup
{
    enum GameObjects
    {
        Player,
        Standing,
        Reward,
        Timer,
    }
    

    enum Buttons
    {
        ScreenTouch,
        Start,
        Title,
    }

    private Stop_MiniGameManager _gameManager;
    private GameObject _player;

    public bool LookBack = false;
    public Button TitleButton;
    public Button StartButton;
    public GameObject RewardUI;
    public TextMeshProUGUI Timer;

    public float _time = 30;

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
        RewardUI = GetObject((int)GameObjects.Reward);

        StartButton.gameObject.BindEvent(StartGame);

        RewardUI.SetActive(false);
        TitleButton.gameObject.SetActive(false);

        CharacterSet();
        return true;
    }
    private void FixedUpdate()
    {
        if (_gameManager.IsGameOver)
        {
            return;
        }

        _time -= Time.deltaTime; // 타이머 감소
        Timer.text = _time.ToString("F0"); // 소수점 첫째 자리까지 표시
    }

    public void StartGame()
    {
        if (_gameManager.IsGameOver)
        {
            return;
        }

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

        if (_gameManager.IsLookBack)
        {
            _gameManager.GameOver();



            GameEndUI(0); // 게임 오버시 보상 UI 띄우기
            return;
        }

        if (_player.transform.localPosition.x > 830)
        { 
            _gameManager.GameClear();
            GameEndUI(1000);
        }

        _player.transform.position += Vector3.right*10;
    }

    public void CharacterSet()
    {
        _player.transform.localPosition = new Vector3(-830, -150, 0);
        _gameManager.transform.localScale = Vector3.one; // 좌우 반전 초기화
    }

    public void GameEndUI(float money)  //타이틀버튼 키고 UI
    {
        RewardUI.SetActive(true);
        TitleButton.gameObject.SetActive(true);
        RewardUI.GetComponentInChildren<TextMeshProUGUI>().text = "Reward\n" + money + "골드";
    }

    public void PopupClose()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
