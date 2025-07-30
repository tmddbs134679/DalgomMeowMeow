using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 건설할 때 롱프레스 게이지를 시각화하는 UI
/// </summary>
public class UI_LongPressGauge : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        MoveUI,
    }

    enum Buttons
    {
    }

    enum Texts
    {
    }

    enum Images
    {
        GaugeImage,
    }
    #endregion

    public float holdDuration = 1.5f;

    private bool wasHolding = false;
    private Image gaugeImage;
    private Sequence gaugeSequence;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        gaugeImage = GetImage((int)Images.GaugeImage);
        gaugeImage.fillAmount = 0f;
        gaugeImage.color = Color.gray;

        return true;
    }

    private void Update()
    {
        var placer = BuildingPlacer.Instance;
        if (placer == null || placer.dragController == null) return;

        // 오브젝트 따라가기
        if (placer.tempDraggleOBJ != null && gameObject.activeSelf)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, placer.tempDraggleOBJ.transform.position);
            GetObject((int)GameObjects.MoveUI).transform.position = screenPos;
        }

        // 게이지 처리
        float timer = placer.dragController.pointerDownTimer;
        bool isHolding = placer.dragController.IsPointDown;

        if (isHolding)
        {
            if (!wasHolding)
            {
                StartGaugeAnimation(); // 최초 진입 시 애니메이션 시작
                wasHolding = true;
            }

            // 실시간으로 fillAmount도 보정 (DOTween보다 우선순위)
            float t = Mathf.Clamp01(timer / holdDuration);
            gaugeImage.fillAmount = t;

            // 수동으로 끝났는지 확인 (DOTween OnComplete 대신)
            if (t >= 1f)
            {
                CompleteGaugeAnimation();
            }
        }
        else
        {
            if (wasHolding)
            {
                ResetGaugeAnimation();
                wasHolding = false;
            }
        }
    }

    private void StartGaugeAnimation()
    {
        gaugeImage.fillAmount = 0f;
        gaugeImage.color = Color.gray;
        gaugeImage.transform.localScale = Vector3.one;

        // 기존 시퀀스 제거
        gaugeSequence?.Kill();

        // DOTween Sequence 시작
        gaugeSequence = DOTween.Sequence();

        gaugeSequence.Append(DOTween.To(
            () => gaugeImage.fillAmount,
            x => gaugeImage.fillAmount = x,
            1f,
            holdDuration
        ).SetEase(Ease.InOutSine));

        gaugeSequence.Join(gaugeImage.DOColor(Color.green, holdDuration));

        gaugeSequence.Join(gaugeImage.transform.DOScale(1.1f, 0.4f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine));

        gaugeSequence.Play();
    }

    private void ResetGaugeAnimation()
    {
        gaugeSequence?.Kill();
        gaugeImage.fillAmount = 0f;
        gaugeImage.color = Color.gray;
        gaugeImage.transform.localScale = Vector3.one;
    }

    private void CompleteGaugeAnimation()
    {
        gaugeSequence?.Kill();
        gaugeImage.fillAmount = 1f;
        gaugeImage.color = Color.green;
        gaugeImage.transform.localScale = Vector3.one;
    }

    // UI hide/show
    public void SetActive(bool istrue)
    {
        gameObject.SetActive(istrue);
    }
}
