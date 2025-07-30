using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FishingText : MonoBehaviour
{
    [SerializeField] private Sprite[] icons; // 아이콘 배열 (0: Miss, 1: Normal, 2: Jackpot)
    private SpriteRenderer iconImage; // 아이콘 이미지 컴포넌트
    private TextMeshPro text;
    

    private float elapsed = 0f;
    private float waveAmplitude = 0.2f;
    private float waveFrequency = 8f;
    private float duration = 2f;

    private Vector3 startPos;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        iconImage = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false); // 초기에는 비활성화
    }
    private void ShowText()
    {
        elapsed = 0f; // 타이머 초기화
        this.transform.localPosition = new Vector3(0f, 2.5f, 0f); // 텍스트 위치 초기화
        this.gameObject.SetActive(true); // 텍스트 활성화
        // 초기 세팅
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        startPos = transform.localPosition;

        Vector3 endPos = startPos + new Vector3(0f, 2.5f, 0f); // 위로 이동

        // 부드럽게 위로 이동 + 페이드아웃
        Sequence seq = DOTween.Sequence()
            .Join(transform.DOLocalMoveY(endPos.y, duration).SetEase(Ease.OutSine))
            .Join(text.DOFade(0, duration))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (Camera.main != null)
        {
            Vector3 rightDir = Camera.main.transform.right;
            Vector3 offset = rightDir * Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;
            transform.localPosition = startPos + offset;
        }
    }

    private void LateUpdate()
    {
        // 항상 카메라 바라보기
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
    [ContextMenu("test")]
    public void test1()
    {
        PlayResult(FishingResult.Normal);
    }
    [ContextMenu("test2")]
    public void test2()
    {
        PlayResult(FishingResult.Jackpot);
    }
    [ContextMenu("test3")]
    public void test3()
    {
        PlayResult(FishingResult.Miss);
    }




    public void PlayResult(FishingResult result)
    {
        switch (result)
        {
            case FishingResult.Miss:
                text.text = "MISS...";
                break;
            case FishingResult.Normal:
                text.text = $"+500";
                Managers.Game.Gold += 500;
                break;
            case FishingResult.Jackpot:
                text.text = $"+100";
                Managers.Game.Dia += 100;
                break;
        }
        iconImage.sprite = icons[(int)result];
        ShowText();
    }
}