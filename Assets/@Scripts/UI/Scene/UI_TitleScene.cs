using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        TextObjectGroup,
        Slider,
    }

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText,
        DaText,
        NaText,
        MaText

    }
    #endregion

    bool isPreload = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            if (isPreload)
                Managers.Scene.LoadScene(Define.EScene.GameScene, transform);
        });

        GetButton((int)Buttons.StartButton).gameObject.SetActive(false);


        return true;
    }
    private void Awake()
    {
        Init();
    }


    private void Start()
    {
        PlayTitleAnimation();

        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count / totalCount;
            if (count == totalCount)
            {
                isPreload = true;
                GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
                Managers.Data.Init();
                Managers.Game.Init();
                Managers.Time.Init();
                StartButtonAnimation();
            }
        });
    }
    void StartButtonAnimation()
    {
        GetText((int)Texts.StartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
    }
    void PlayTitleAnimation()
    {
        var da = GetText((int)Texts.DaText).rectTransform;
        var na = GetText((int)Texts.NaText).rectTransform;
        var ma = GetText((int)Texts.MaText).rectTransform;

        Sequence seq = DOTween.Sequence();

        // 달곰
        seq.Append(PlayBouncyAnimation(da));
        seq.AppendInterval(0.3f); 

        // 냥냥
        seq.Append(PlayBouncyAnimation(na));
        seq.AppendInterval(0.3f);

        // 마을
        seq.Append(PlayBouncyAnimation(ma));
    }

    Sequence PlayBouncyAnimation(RectTransform target)
    {
        Sequence bounceSeq = DOTween.Sequence();

        Vector3 originalScale = target.localScale;
        Vector3 punchScale = originalScale * 2f;

        // 스케일 업 + 흔들림
        bounceSeq.Append(target.DOScale(punchScale, 0.3f).SetEase(Ease.OutBack));
        bounceSeq.Join(target.DOShakeAnchorPos(0.3f, new Vector2(10f, 0), 10, 90f, false, true));

        // 스케일 복원
        bounceSeq.Append(target.DOScale(originalScale, 0.2f).SetEase(Ease.InOutQuad));

        return bounceSeq;
    }
}
