using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_TextAnimation : UI_Popup
{
    #region Enum

    enum Texts
    {
        AnimText,
    }

    #endregion


    public EBuildingType _buildingType;  // 현재 건물의 종류
    CanvasGroup _canvasGroup;
    // Start is called before the first frame update

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _canvasGroup = GetComponent<CanvasGroup>();
        BindText(typeof(Texts));

        return true;
    }
    void Start()
    {
        // Enum 값에 따라 애니메이션 실행
        switch (_buildingType)
        {
            case EBuildingType.Cooking:
                GetText((int)Texts.AnimText).text = Define.COOK_TEXT;
                CookingBuildingAnimation();

                break;
            case EBuildingType.OnionFarm:
            case EBuildingType.PotatoFarm:
            case EBuildingType.CarrotFarm:
            case EBuildingType.CabbageFarm:
                GetText((int)Texts.AnimText).text = Define.FARM_TEXT;
                FarmingBuildingAnimation();

                break;
            case EBuildingType.Resting:
                GetText((int)Texts.AnimText).text = Define.REST_TEXT;
                RestBuildingAnimation();

                break;
            case EBuildingType.Playing:
                GetText((int)Texts.AnimText).text = Define.PLAYING_TEXT;
                PlayGroundAnimation();
                break;
        }
    }

    public void SetInfo(EBuildingType type, Vector3 position)
    {
        _buildingType = type;
        position.y += 2;
        transform.position = position;
    }

    float moveDuration = 2f;  // 원을 도는 시간
    float radius = 1f;      // 원의 반지름
    Sequence seq;

    Vector3 cameraRotation = new Vector3(0f, 0f, 0f);
    // 요리 건물 애니메이션
    void CookingBuildingAnimation()
    {
        Vector3 centerPosition = transform.position;  // 텍스트의 현재 위치 (원점)
        Vector3[] path = new Vector3[360];
        Quaternion rotation = Quaternion.Euler(cameraRotation);

        // 경로 생성
        for (int i = 0; i < 360; i++)
        {
            float radian = i * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(radian) * radius, 0, Mathf.Sin(radian) * radius);
            path[i] = centerPosition + rotation * point;
        }


        // 처음에는 완전 투명
        _canvasGroup.alpha = 0;

        float waitTime = 1.5f; // 한 바퀴 끝나고 대기 시간 (초)

        //  기존 시퀀스 초기화
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // 기존 시퀀스 제거
        }

        // 새 시퀀스 생성
        seq = DOTween.Sequence();

        // 한 바퀴 돌기 전에 alpha를 1로
        seq.AppendCallback(() =>
        {
            _canvasGroup.alpha = 1f; // 돌기 시작할 때 켜짐
        });

        // 원형 경로 따라 이동
        Tween pathTween = transform.DOPath(path, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear);

        // 진행률로 알파값 조절
        pathTween.OnUpdate(() =>
        {
            float progress = pathTween.ElapsedPercentage(); // DOPath 한 바퀴 기준 진행률 (0~1)

            if (progress < 0.5f)
            {
                // 절반까지는 그대로 보임
                _canvasGroup.alpha = 1f;
            }
            else
            {
                // 절반 이후 점점 사라짐
                float fade = Mathf.Lerp(1f, 0f, (progress - 0.5f) * 2f);
                _canvasGroup.alpha = fade;
            }
        });

        // 시퀀스에 추가
        seq.Append(pathTween);

        // 한 바퀴 끝나면 alpha를 0으로
        seq.AppendCallback(() =>
        {
            _canvasGroup.alpha = 0f; // 한 바퀴 끝나면 완전히 사라짐
        });

        // 대기 시간 동안 사라진 상태 유지
        seq.AppendInterval(waitTime);

        // 무한 반복
        seq.SetLoops(-1, LoopType.Restart);
    }

    // 밭 건물 애니메이션
    void FarmingBuildingAnimation()
    {
        // 초기 세팅 (투명도 0, 크기 1)
        _canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one;

        // 기존 시퀀스 초기화
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        // 나타나기
        seq.Append(_canvasGroup.DOFade(1f, 0.1f)); // 매우 빠르게 나타남

        //  팝 효과 연출
        seq.Join(transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutBack)); 
        seq.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.OutCubic)); 

        //  잠깐 유지
        seq.AppendInterval(1f);

        // 사라짐
        seq.Append(_canvasGroup.DOFade(0f, 0.1f));

        // 잠깐 유지
        seq.AppendInterval(0.2f);

        seq.SetLoops(-1, LoopType.Restart);
    }

    // 잠자는 건물 애니메이션
    void RestBuildingAnimation()
    {
        // 시작 위치 기억
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0, 1.5f, 0); // 위로 50f 이동

        _canvasGroup.alpha = 1f;
        transform.localPosition = startPosition;


        if (seq != null && seq.IsActive())
        {
            seq.Kill();
        }


        seq = DOTween.Sequence();

        // 동시에: 위로 이동 + 점점 사라짐
        seq.Append(transform.DOLocalMove(endPosition, 1.5f).SetEase(Ease.OutSine));
        seq.Join(_canvasGroup.DOFade(0f, 1.5f)); // 처음엔 1, 1.5초 동안 0으로



        seq.AppendInterval(1f);

        // 위치 초기화
        seq.AppendCallback(() =>
        {
            transform.localPosition = startPosition;
            _canvasGroup.alpha = 1f; // 다시 보이게 초기화
        });

        // 무한 반복
        seq.SetLoops(-1, LoopType.Restart);
    }

    void PlayGroundAnimation()
    {
        // 초기 위치 기억
        Vector3 startPosition = transform.localPosition;
        
        transform.localScale = Vector3.one;

        //초기화
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        seq.AppendInterval(0.1f);

        seq.Append(transform.DOLocalMoveY(startPosition.y + 1.5f, 0.6f).SetEase(Ease.OutQuad));

        seq.Join(transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack));
        seq.Join(transform.DOScale(1f, 0.5f).SetEase(Ease.InOutCubic));

        seq.Append(transform.DOLocalMoveY(startPosition.y, 0.6f).SetEase(Ease.InQuad));


        seq.SetLoops(-1, LoopType.Restart);
    }

}
