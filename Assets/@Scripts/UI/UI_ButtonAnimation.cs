using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonAnimation : UI_Base
{
    private void Start()
    {
        gameObject.BindEvent(ButtonPointerDownAnimation, type: Define.EUIEvent.PointerDown);
        gameObject.BindEvent(ButtonPointerUpAnimation, type : Define.EUIEvent.PointerUp);
    }

    private void ButtonPointerUpAnimation()
    {
        transform.DOScale(1f, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    private void ButtonPointerDownAnimation()
    {
        transform.DOScale(0.85f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true);
    }
}
