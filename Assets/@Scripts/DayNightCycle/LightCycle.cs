using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections;
public class LightCycle : MonoBehaviour
{
    [SerializeField] private float _adjustedNormalizedTime;
    [SerializeField] private Light _targetLight;
    [SerializeField] private float duration;
    private Coroutine currentCoroutine;
    [SerializeField] private bool hasTurnedOn = false;
    [SerializeField] private bool hasTurnedOff = false;
    [SerializeField] private float Changeparameter;
    void Awake()
    {
        _targetLight = this.GetComponent<Light>();
    }

    void Start()
    {
        duration = GetFadeDuration();
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

        // 0.45f를 지나면 한 번만 켜기
        if (!hasTurnedOn && t >= 0.45f && t < 0.95f)
        {
            FadeLight(true);
            hasTurnedOn = true;
        }

        // 0.95f를 지나면 한 번만 끄기
        if (!hasTurnedOff && t >= 0.95f)
        {
            FadeLight(false);
            hasTurnedOff = true;
        }

        // 사이클 반복 시 플래그 초기화
        if (t < 0.10f)
        {
            hasTurnedOn = false;
            hasTurnedOff = false;
        }
    }


    public void FadeLight(bool turnOn)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeLightCoroutine(turnOn));
    }

    IEnumerator FadeLightCoroutine(bool turnOn)
    {
        float start = _targetLight.intensity;
        float end = turnOn ? Changeparameter : 0f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _targetLight.intensity = Mathf.Lerp(start, end, t);
            yield return null;
        }

        _targetLight.intensity = end;
    }


    private float GetFadeDuration()
    {
        // 하루 길이에 따라 페이드 시간 결정
        // 기준: 480초일 때 2초, 60초일 때 0.25초
        float normalized = Mathf.InverseLerp(20f, 480f, duration);
        return Mathf.Lerp(1f, 24f, normalized);
    }
}
