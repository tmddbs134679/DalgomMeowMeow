using UnityEngine;
using UnityEngine.UI;

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
    private Transform gaugeTransform;

    private float scaleAnimTime = 0f;
    private float scaleAnimSpeed = 2f; // 진동 속도
    private float scaleAnimAmount = 0.05f; // 진동 강도

    private Color startColor = new Color(0.8f, 0.8f, 0.8f, 0.3f);
    private Color fillColor = new Color(0.2f, 1f, 0.7f, 0.85f);
    private Color completeColor = Color.green;

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
        gaugeTransform = gaugeImage.transform;

        gaugeImage.fillAmount = 0f;
        gaugeImage.color = startColor;

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

        // 게이지 채우기
        float timer = placer.dragController.pointerDownTimer;
        bool isHolding = placer.dragController.IsPointDown;

        if (isHolding)
        {
            if (!wasHolding)
            {
                StartGaugeAnimation();
                wasHolding = true;
            }

            float t = Mathf.Clamp01(timer / holdDuration);
            gaugeImage.fillAmount = t;
            gaugeImage.color = Color.Lerp(startColor, fillColor, t);

            // 흔들림 효과
            scaleAnimTime += Time.deltaTime * scaleAnimSpeed;
            float scale = 1f + Mathf.Sin(scaleAnimTime) * scaleAnimAmount;
            gaugeTransform.localScale = new Vector3(scale, scale, 1f);

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
        gaugeImage.color = startColor;
        gaugeTransform.localScale = Vector3.one;
        scaleAnimTime = 0f;
    }

    private void ResetGaugeAnimation()
    {
        gaugeImage.fillAmount = 0f;
        gaugeImage.color = startColor;
        gaugeTransform.localScale = Vector3.one;
    }

    private void CompleteGaugeAnimation()
    {
        gaugeImage.fillAmount = 1f;
        gaugeImage.color = completeColor;
        gaugeTransform.localScale = Vector3.one;
    }

    public void SetActive(bool istrue)
    {
        if (gameObject.activeSelf == istrue) return;
        gameObject.SetActive(istrue);
    }
}
