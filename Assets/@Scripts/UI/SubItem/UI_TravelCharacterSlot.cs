using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_TravelCharacterSlot : UI_Base
{
    #region Enum
    enum GameObjects
    {

    }

    enum Buttons
    {

    }

    enum Texts
    {
        CharacterNameText
    }

    enum Images
    {
        CharacterImage
    }
    #endregion

    bool _isDrag;
    ScrollRect _scrollRect;
    Outline _outline;
    public Character _character;

    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        _outline = GetComponent<Outline>();
        gameObject.BindEvent(OnClickSlot);

        gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        return true;
    }

    private void OnClickSlot()
    {
        Managers.UI.OnTravelCharacter?.Invoke(this);
    }

    public void SetInfo(Character character, ScrollRect scrollrect)
    {
        _scrollRect = scrollrect;
        _character = character;

        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        GetText((int)Texts.CharacterNameText).text = _character.Name;
    }

    public void SetOutline(bool active)
    {
        _outline.enabled = active;
    }




    public void OnDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
}
