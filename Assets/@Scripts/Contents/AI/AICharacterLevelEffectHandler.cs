using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AICharacterLevelEffectHandler : MonoBehaviour
{
    [Header("파티클")]
     private ParticleSystem levelUpEffect;

    [Header("월드 스페이스 UI")]
     private GameObject levelCanvas;
     private TextMeshProUGUI levelUpText;

    private Vector3 levelTextInitialPos;
    private Color levelTextInitialColor;

    public void Init()
    {
        levelCanvas = transform.Find("Level").gameObject;
        levelUpEffect = transform.Find("Particle").GetComponent<ParticleSystem>();
        levelUpText = transform.Find("Level/LevelUpText").GetComponent<TextMeshProUGUI>();
        levelTextInitialPos = levelUpText.transform.localPosition;
        levelTextInitialColor = levelUpText.color;

        // 시작 시 비활성화
        levelCanvas.SetActive(false);
    }

    public void PlayLevelUpEffect(Camera mainCam)
    {
        // 파티클 재생
        if (levelUpEffect != null)
        {
            levelUpEffect.Stop();
            levelUpEffect.Play();
        }

        // 텍스트 위치 리셋 & 색상 리셋
        levelUpText.transform.localPosition = levelTextInitialPos;
        levelUpText.color = levelTextInitialColor;

        // 캔버스 활성화 + 카메라 바라보게
        levelCanvas.SetActive(true);
        levelUpText.transform.forward = mainCam.transform.forward;

        // 트윈 애니메이션
        Sequence seq = DOTween.Sequence();
        seq.Append(levelUpText.transform.DOLocalMoveY(levelTextInitialPos.y + 1f, 1f));
        seq.Join(levelUpText.DOFade(0f, 1f));
        seq.OnComplete(() =>
        {
            levelCanvas.SetActive(false);
        });
    }
}
