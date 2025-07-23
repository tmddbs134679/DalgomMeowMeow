using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColorGradient;
    [SerializeField] private AnimationCurve intensityCurve;
    [SerializeField] private float dayDurationInSeconds = 480f; // 8분
[SerializeField] [Range(0f, 1f)] private float dayRatio = 0.68f;
    private float cycleTimer = 0f;

    private bool _isDay = true;
    
    public bool IsDay
    {
        get
        {
            if (!PlayerPrefs.HasKey("IsDay"))
            {
                _isDay = true;
                PlayerPrefs.SetInt("IsDay", 1);
                PlayerPrefs.Save();
            }
            else
            {
                _isDay = PlayerPrefs.GetInt("IsDay") == 1;
            }

            return _isDay;
        }
        set
        {
            _isDay = value;
            PlayerPrefs.SetInt("IsDay", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        if (Managers.Time != null)
        {
            TimeSpan sinceLast = DateTime.Now - Managers.Time.LastDayNightCheckTime;
            cycleTimer = (float)sinceLast.TotalSeconds % dayDurationInSeconds;
        }

        InvokeRepeating(nameof(CheckDayNightCycle), 0f, 1f); // 매초 확인
    }

void Update()
{
    cycleTimer += Time.deltaTime;

    float normalizedTime = (cycleTimer % dayDurationInSeconds) / dayDurationInSeconds;

float sunAngle;
if (normalizedTime < dayRatio)
{
    // 낮: 천천히 진행 (70 → 240도)
    sunAngle = Mathf.Lerp(60f,175f, normalizedTime / dayRatio);
}
else
{
    // 밤: 빠르게 진행 (240 → 430도 = 70 + 360)
    sunAngle = Mathf.Lerp(175f, 420f, (normalizedTime - dayRatio) / (1f - dayRatio));
}
sunAngle %= 360f; // 0~360도로 제한

directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
float adjustedNormalizedTime = sunAngle / 360f;
directionalLight.color = lightColorGradient.Evaluate(adjustedNormalizedTime);
directionalLight.intensity = Mathf.Max(intensityCurve.Evaluate(adjustedNormalizedTime), 0.001f);
}

    void CheckDayNightCycle()
    {
        TimeSpan timeSinceLastCheck = DateTime.Now - Managers.Time.LastDayNightCheckTime;

        if (timeSinceLastCheck.TotalMinutes >= 8)
        {
            // SwitchDayNight();
            Managers.Time.LastDayNightCheckTime = DateTime.Now;
        }
    }

    void SwitchDayNight()
    {
        IsDay = !IsDay;

        if (IsDay)
        {
            Debug.Log(" 낮으로 전환");
        }
        else
        {
            Debug.Log(" 밤으로 전환");
        }
    }
}
