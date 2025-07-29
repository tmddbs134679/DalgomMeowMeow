using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MoleManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite _cat;
    [SerializeField] private Sprite _shark;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float gameTime = 30f;  // 총 게임 시간
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private GameObject _textUI;
    [SerializeField] private GameObject _scoreUI;

    public float MoleDuration = 1.0f; // 몰이 올라와 있는 시간 (초 단위)

    private float _spawnInterval = 1.0f;
    private float _elapsedTime = 0f;

    private bool[] isActive; // 올라온 상태
    private bool isGameStarted = false;

    private int _score;
    private float HighScore
    {
        get => PlayerPrefs.GetFloat("HighScore", 0);
        set
        {
            PlayerPrefs.SetFloat("HighScore", value);
            PlayerPrefs.Save();
        }
    }
    private int Score
    {
        get => _score;
        set
        {
            _score = Mathf.Max(0, value); // 0 이하로 안내려가게 제한
            RenewText(); // 점수 바뀔 때마다 UI 자동 갱신
        }
    }

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        isActive = new bool[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            int idx = i; 
            buttons[i].onClick.AddListener(() => OnClickMole(idx));
        }
    }

    public void StartGame()
    {
        Score = 0;
        RenewText();
        isGameStarted = true;
        StartCoroutine(SpawnLoop());
        StartCoroutine(TimerCoroutine());
    }

    public void GameOver()
    {
        isGameStarted = false; // 게임 루프 멈추는 기준
        StopAllCoroutines(); // 모든 코루틴 중지
        _textUI.SetActive(false); // 게임 오버 UI 활성화
        if(_score > HighScore)
        {
            HighScore = _score; // 새로운 최고 점수 갱신
        }
        _scoreUI.GetComponentInChildren<TextMeshProUGUI>().text = $"HighScore\n{HighScore}\nScore\n{_score}";
        _scoreUI.transform.Find("Reward").GetComponent<TextMeshProUGUI>().text = $"Reward\n{HighScore * 0.5} 골드"; // 보상 텍스트 갱신
        _scoreUI.SetActive(true); // 점수 UI 활성화
    }

    private void UpdateTimerUI()
    {
        int timeLeft = Mathf.CeilToInt(gameTime - _elapsedTime);
        _timeText.text = $"{timeLeft}";
    }

    private void TryPopMole()
    {
        List<int> candidates = new();

        for (int i = 0; i < isActive.Length; i++)
        {
            if (!isActive[i])
                candidates.Add(i);
        }

        if (candidates.Count == 0) return;

        int index = candidates[Random.Range(0, candidates.Count)];
        StartCoroutine(CharacterRoutine(index));
    }

    private void OnClickMole(int index)
    {
        var mole = buttons[index].GetComponent<Mole_Onclick>();
        if (mole.isClicked) return;

        mole.Clicked();

        if (buttons[index].image.sprite == _cat)
        {
            Score -= 500;
            Managers.Sound.Play(Define.ESound.Effect, "Cat1");
        }
        else
        {
            Score += 200;
            Managers.Sound.Play(Define.ESound.Effect, "Click3");
        }

    }

    private void RenewText()
    {
        _scoreText.text = $"{Score}";
    }

    private IEnumerator SpawnLoop()
    {
        _spawnInterval = 1.0f;

        while (isGameStarted)
        {
            int count = Random.Range(1, 4); // 동시 등장 수

            for (int i = 0; i < count; i++)
                TryPopMole();

            yield return new WaitForSeconds(_spawnInterval);

            _spawnInterval = Mathf.Max(0.3f, _spawnInterval - 0.02f);
        }
    }
    private IEnumerator TimerCoroutine()
    {
        while (_elapsedTime < gameTime)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerUI();
            yield return null; // 매 프레임마다 갱신
        }

        GameOver(); // 시간이 다 되면 게임 종료
    }
    private IEnumerator CharacterRoutine(int index)
    {
        isActive[index] = true;
        var btn = buttons[index];
        var mole = btn.GetComponent<Mole_Onclick>();

        mole.isClicked = false;
        btn.interactable = true;

        // 랜덤 캐릭터 배정
        buttons[index].image.sprite = (Random.value < 0.33f) ? _cat : _shark;

        // 올라오기
        yield return btn.transform.DOLocalMoveY(200, 0.3f).SetEase(Ease.OutBounce).WaitForCompletion();

        float timer = 0f;
        while (timer < MoleDuration)
        {
            if (mole.isClicked)
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        // 내려가기
        yield return btn.transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.InBack).WaitForCompletion();
        btn.interactable = false;
        isActive[index] = false;
    }
}