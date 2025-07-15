using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stop_MiniGameManager : MonoBehaviour
{
    public enum ForwardTime
    {
        Slowest = 2,
        Slow = 3,
        Moderate = 4,
        Fast = 5,
        Fastest = 6,
    }
    public bool IsLookBack = false;
    public bool Isblink = false; //깜빡임 여부
    public float LookBackTime = 3f;
    public float time;

    public bool IsGameOver = false;
    


    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (IsGameOver) return;
        if (IsLookBack)
        {
            LookBackTime -= Time.deltaTime;
            if (LookBackTime < 0)
            {
                IsLookBack = false;
                GameStart();
            }
        }
    }

    public int RandomTiming()
    {
        int k = Random.Range(2, 7);     //2~6까지 랜덤
        return k;
    }

    [ContextMenu("GameStart")]
    public void GameStart()
    {
        Flip(); // 좌우 반전(앞에봄)
        float time = RandomTiming();
        StartCoroutine(GameStartCoroutine(time));
    }

    IEnumerator GameStartCoroutine(float time)
    {
        if (time > 1f)
            yield return new WaitForSeconds(time - 1f); // 깜빡임 시작까지 대기

        StartBlinkEffect(1f); // 1초 동안 깜빡임
    }

    void StartBlinkEffect(float duration)
    {
        Isblink = true;

        _image.DOKill();

        _image.DOFade(0f, 0.15f)
            .SetLoops((int)(duration / 0.3f), LoopType.Yoyo)
            .OnComplete(() =>
            {
                _image.color = new Color(1f, 1f, 1f, 1f); // 최종 복구
                Isblink = false;
                LookBack();

            });
    }

    public void LookBack()
    {
        Flip(); // 좌우 반전
        Isblink = false;
        IsLookBack = true;
        LookBackTime = Random.Range(1,5);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;  // 좌우 반전
        transform.localScale = scale;
    }


    public void GameOver()
    {
        IsGameOver = true;
        StopAllCoroutines(); // 현재 진행 중인 코루틴 모두 중지
        _image.DOKill(); // DOTween 트윈도 정리



    }

    public void GameClear()
    {
        IsGameOver = true;
        StopAllCoroutines(); // 현재 진행 중인 코루틴 모두 중지
        _image.DOKill(); // DOTween 트윈도 정리
        //보상


    }


}
