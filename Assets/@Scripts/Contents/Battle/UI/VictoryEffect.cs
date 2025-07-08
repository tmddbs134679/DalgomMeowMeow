using DG.Tweening;
using UnityEngine;

public class VictoryEffect : MonoBehaviour
{
    private Transform _target;

    void Start()
    {
        _target = transform;

        // 1단계: 처음엔 스케일 0으로 시작
        _target.localScale = Vector3.zero;

        // 2단계: 팡 하고 커짐 → 그 다음에 반복 애니메이션 시작
        _target.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(StartIdleBounce);
    }

    private void StartIdleBounce()
    {
        // 3단계: 살짝 커졌다 작아졌다를 무한 반복
        _target.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}