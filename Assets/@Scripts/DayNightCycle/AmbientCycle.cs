using System.Collections;
using UnityEngine;

public class AmbientCycle : MonoBehaviour
{
    [SerializeField] private float _adjustedNormalizedTime;
    [SerializeField] private float duration = 2f; // 기본 페이드 시간 (초)
    private Coroutine currentCoroutine;
    [SerializeField] private bool hasTurnedOn = false;
    [SerializeField] private bool hasTurnedOff = false;
    [SerializeField] private float changeParameter = 1f; // 켤 때 목표 ambientIntensity 값
    private float originalAmbientIntensity;

    [SerializeField] private float minEnd;
        [SerializeField] private float minT;
            [SerializeField] private float maxT;

    void Start()
    {
        duration = GetFadeDuration();
    }

    void Update()
    {
        if (DayNightCycleManager.Instance != null)
            _adjustedNormalizedTime = DayNightCycleManager.Instance.AdjustedNormalizedTime;

        AmbientCycleCalculate();
    }

    void AmbientCycleCalculate()
    {
        float t = _adjustedNormalizedTime;

        // 0.45f를 지나면 한 번만 켜기
        if (!hasTurnedOn && t >= minT && t <maxT)
        {
            FadeAmbient(true);
            hasTurnedOn = true;
        }

        // 0.95f를 지나면 한 번만 끄기
        if (!hasTurnedOff && t >= maxT)
        {
            FadeAmbient(false);
            hasTurnedOff = true;
        }

        // 사이클 반복 시 플래그 초기화
        if (t < 0.01f)
        {
            hasTurnedOn = false;
            hasTurnedOff = false;
        }
    }

    public void FadeAmbient(bool turnOn)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(FadeAmbientCoroutine(turnOn));
    }

    IEnumerator FadeAmbientCoroutine(bool turnOn)
    {
        float start = RenderSettings.ambientIntensity;
        float end = turnOn ? changeParameter :minEnd;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            RenderSettings.ambientIntensity = Mathf.Lerp(start, end, t);
            yield return null;
        }

        RenderSettings.ambientIntensity = end;
    }

    private float GetFadeDuration()
    {
        // 하루 길이에 따라 페이드 시간 결정
        // 기준: 480초일 때 2초, 60초일 때 0.25초 (임의 설정)
        // 여기 duration 변수 사용하지 않고 하드코딩하거나 수정 가능
        float normalized = Mathf.InverseLerp(20f, 480f, duration);
        return Mathf.Lerp(2f, 24f, normalized);
    }
}
