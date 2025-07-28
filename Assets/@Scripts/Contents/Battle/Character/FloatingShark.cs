using DG.Tweening;
using UnityEngine;

public class FloatingShark : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(Float), Random.Range(0f , 1f)); // 0.5초 후에 Float 메소드 호출
    }

    public void Float()
    {
        // 기준 위치
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // 시작 위치 세팅
        transform.position = startPos;
        float offset = Random.Range(0.05f, 0.15f);

        // 부유 애니메이션 (y축으로 반복 이동)
        transform.DOMoveY(transform.position.y + offset, 1.5f)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetUpdate(true); // 타임스케일 무시
    }
}
