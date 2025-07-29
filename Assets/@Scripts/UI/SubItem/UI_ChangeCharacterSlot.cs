using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_ChangeCharacterSlot : UI_Base
{
    #region Enum
    enum GameObjects
    {

    }

    enum Buttons
    {

    }

    enum Images
    {
        CharacterIcon,
        MoveSpeedIcon,
    }
    //
    enum Texts
    {
        MoveSpeed,
    }

    #endregion

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
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        gameObject.BindEvent(OnClickObjectButton);
        return true;
    }

    private void OnClickObjectButton()
    {
        Managers.UI.GetPopupUI<UI_ChangePopup>().OnClickSlot(_character);
        var outLine = GetImage((int)Images.CharacterIcon).GetComponent<Outline>();
        outLine.enabled = _character.InMainScene;
        
    }

    Character _character;
    ScrollRect _scrollRect;
    bool _isDrag = false;

    public void SetInfo(Character character, ScrollRect scrollRect)
    {
        _character = character;
        _scrollRect = scrollRect;

        GetImage((int)Images.CharacterIcon).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        var outLine = GetImage((int)Images.CharacterIcon).GetComponent<Outline>();
        outLine.enabled = _character.InMainScene;


        GetImage((int)Images.MoveSpeedIcon).sprite = Managers.Resource.Load<Sprite>("Bread");

        GetText((int)Texts.MoveSpeed).text = _character.MoveSpeed.ToString();
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
