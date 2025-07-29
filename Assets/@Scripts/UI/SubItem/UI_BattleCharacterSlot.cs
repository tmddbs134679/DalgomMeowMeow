using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_BattleCharacterSlot : UI_Base
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
        HealthIcon,
        AtkIcon,
        SkillIcon,
    }

    enum Texts
    {
        Health,
        Atk,
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
        Managers.UI.GetPopupUI<UI_ForestPopup>().SelectCharacter(_character);
    }

    Character _character;
    ScrollRect _scrollRect;
    bool _isDrag = false;

    public void SetInfo(Character character, ScrollRect scrollRect = null)
    {
        _character = character;
        _scrollRect = scrollRect;

        GetImage((int)Images.CharacterIcon).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        GetImage((int)Images.HealthIcon).sprite = Managers.Resource.Load<Sprite>("Cheese");
        GetImage((int)Images.AtkIcon).sprite = Managers.Resource.Load<Sprite>("Fork");
        GetImage((int)Images.SkillIcon).sprite = Managers.Resource.Load<Sprite>(_character.Data.SkillID.ToString());

        GetText((int)Texts.Health).text = _character.Hp.ToString();
        GetText((int)Texts.Atk).text = _character.Atk.ToString();
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
