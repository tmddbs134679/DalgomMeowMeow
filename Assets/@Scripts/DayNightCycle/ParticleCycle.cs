using System.Collections;
using UnityEngine;

public class ParticleCycle : MonoBehaviour
{
    [SerializeField] private float _adjustedNormalizedTime;
    [SerializeField] private ParticleSystem _targetParticleSystem;
    [SerializeField] private float duration;
    private Coroutine currentCoroutine;
    [SerializeField] private bool hasTurnedOn = false;
    [SerializeField] private bool hasTurnedOff = false;
    [SerializeField] private float Changeparameter = 20f; // 최대 파티클 수

    void Awake()
    {
        _targetParticleSystem = GetComponent<ParticleSystem>();
    }

void Start()
{
    duration = GetFadeDuration();

    // 기본 rate 설정
    var rate = _targetParticleSystem.emission.rateOverTime;
    rate.constant = 10f;

    // Lifetime 계산 및 설정
    float normalized = Mathf.InverseLerp(20f, 480f, DayNightCycleManager.Instance.DayDurationInSeconds);
    float lifetime = Mathf.Lerp(2f, 6f, normalized); // 예: 0.5초 ~ 6초

    var main = _targetParticleSystem.main;
    main.startLifetime = lifetime;
}


    void Update()
    {
        if (DayNightCycleManager.Instance != null)
            _adjustedNormalizedTime = DayNightCycleManager.Instance.AdjustedNormalizedTime;

        SunCycleCalculate();
    }

    void SunCycleCalculate()
    {
        float t = _adjustedNormalizedTime;

        if (!hasTurnedOn && t >= 0.50f && t < 0.90f)
        {
            FadeParticles(true);
            hasTurnedOn = true;
        }

        if (!hasTurnedOff && t >= 0.90f)
        {
            FadeParticles(false);
            hasTurnedOff = true;
        }

        if (t < 0.10f)
        {
            hasTurnedOn = false;
            hasTurnedOff = false;
        }
    }

    public void FadeParticles(bool turnOn)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeParticlesCoroutine(turnOn));
    }

    IEnumerator FadeParticlesCoroutine(bool turnOn)
    {
        var emission = _targetParticleSystem.emission;
        float startRate = emission.rateOverTime.constant;
        float endRate = turnOn ? Changeparameter : 0f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float currentRate = Mathf.Lerp(startRate, endRate, t);

            var rate = emission.rateOverTime;
            rate.constant = currentRate;
            emission.rateOverTime = rate;
            yield return null;
        }

        var finalRate = emission.rateOverTime;
        finalRate.constant = endRate;
        emission.rateOverTime = finalRate;
    }

    private float GetFadeDuration()
    {
        // 기준: 480초일 때 24초, 20초일 때 1초
        float normalized = Mathf.InverseLerp(20f, 480f, DayNightCycleManager.Instance.DayDurationInSeconds);
        return Mathf.Lerp(1f, 24f, normalized);
    }
}
