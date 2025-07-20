using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColorGradient;
    [SerializeField] private AnimationCurve intensityCurve;
    [SerializeField] private float dayDurationInSeconds = 480f; // 8분

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

        // 회전값 적용
        float sunAngle = normalizedTime * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));

        // 조도 및 색상 변화 적용
        directionalLight.color = lightColorGradient.Evaluate(normalizedTime);
        directionalLight.intensity = intensityCurve.Evaluate(normalizedTime);
    }

    void CheckDayNightCycle()
    {
        TimeSpan timeSinceLastCheck = DateTime.Now - Managers.Time.LastDayNightCheckTime;

        if (timeSinceLastCheck.TotalMinutes >= 8)
        {
            SwitchDayNight();
            Managers.Time.LastDayNightCheckTime = DateTime.Now;
        }
    }

    void SwitchDayNight()
    {
        IsDay = !IsDay;

        if (IsDay)
        {
            Debug.Log("☀️ 낮으로 전환");
        }
        else
        {
            Debug.Log("🌙 밤으로 전환");
        }
    }
}
