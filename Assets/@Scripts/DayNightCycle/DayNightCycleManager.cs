using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DayNightCycleManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColorGradient;
    [SerializeField] private AnimationCurve intensityCurve;
    [SerializeField] private float _dayDurationInSeconds;
    public float DayDurationInSeconds { get => _dayDurationInSeconds; set => _dayDurationInSeconds = value; }
    [SerializeField, Range(0f, 1f)] private float dayRatio;

    [SerializeField] private float _adjustedNormalizedTime;
    public float AdjustedNormalizedTime { get => _adjustedNormalizedTime; set => _adjustedNormalizedTime = value; }
    public static DayNightCycleManager Instance;

    [SerializeField] private bool isDay = false;
    [SerializeField] private bool isNight = false;
        private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 씬 로드 시 콜백 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 로드 콜백 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 새로 로드될 때 호출됨
        // 예: directionalLight 다시 찾기
        directionalLight =  GameObject.FindWithTag("Sun")?.GetComponent<Light>();
    }
    void Start()
    {
        InvokeRepeating(nameof(SaveCurrentTime), 0f, 5f);
    }

    void SaveCurrentTime()
    {
        //주기적으로 갱신
        Managers.Time.LastDayNightCheckTime = Managers.Time.LastDayNightCheckTime; // 강제 save
    }

    void Update()
    {
        if (isDay)
        {
            ApplyFixedTime(0.15f); 
            return;
        }
        if (isDay)
        {
            ApplyFixedTime(0.75f); 
            return;
        }
        TimeSpan elapsed = DateTime.Now - Managers.Time.LastDayNightCheckTime;
        float cycleTimer = (float)(elapsed.TotalSeconds % _dayDurationInSeconds);
        SunCycleCalculate(cycleTimer);

    }

    void SunCycleCalculate(float cycleTimer)
    {
        float normalizedTime = cycleTimer / _dayDurationInSeconds;

        float sunAngle = normalizedTime < dayRatio
            ? Mathf.Lerp(80f, 175f, normalizedTime / dayRatio)
            : Mathf.Lerp(175f, 440f, (normalizedTime - dayRatio) / (1f - dayRatio));

        sunAngle %= 360f;

        if (directionalLight == null) return;
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
        AdjustedNormalizedTime = sunAngle / 360f;
        directionalLight.color = lightColorGradient.Evaluate(AdjustedNormalizedTime);
        directionalLight.intensity = Mathf.Max(intensityCurve.Evaluate(AdjustedNormalizedTime), 0.001f);
    }
    
    void ApplyFixedTime(float normalizedTime)
{
    float cycleTimer = normalizedTime * _dayDurationInSeconds;
    SunCycleCalculate(cycleTimer);
}
}
