using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UI_ProfilePopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        ContentObject,
        EquippedGroupObject,
        EquippedIHattem,
        EquippedAccessoryItem,
        EquippedBagItem
    }

    enum Buttons
    {
        NextCharacterButton,
        PrevCharacterButton,
        ExitButton,
    }

    enum Texts
    {
        CharacterNameText
    }

    enum Images
    {
        Image
    }
    #endregion

    Character _character;
    AICharacter _characterAI;
    UI_EquipItem _equipHatItem;
    UI_EquipItem _equipAccessoryItem;
    UI_EquipItem _equipBagItem;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.PrevCharacterButton).gameObject.BindEvent(() => OnClickChangeButton(-1));
        GetButton((int)Buttons.NextCharacterButton).gameObject.BindEvent(() => OnClickChangeButton(1));

        //GetRawImage((int)Images.Image).


        _equipHatItem = GetObject((int)GameObjects.EquippedIHattem).GetComponent<UI_EquipItem>();
        _equipAccessoryItem = GetObject((int)GameObjects.EquippedAccessoryItem).GetComponent<UI_EquipItem>();
        _equipBagItem = GetObject((int)GameObjects.EquippedBagItem).GetComponent<UI_EquipItem>();

        _character = Managers.Game.Characters[0];

        //Refresh();

        return true;
    }

    private void OnClickChangeButton(int dir)
    {

        // Todo : 다음 캐릭터, 이전 캐릭터
        List<Character> characterList = Managers.Game.Characters;

        if (characterList == null || characterList.Count == 0 || _character == null)
            return;

        // 중복 캐릭터면 고유 id를 사용해야하나? 생각
        int currentIndex = characterList.FindIndex(c => c.UniqueId == _character.UniqueId);
        if (currentIndex == -1)
            return;

        int nextIndex = (currentIndex + dir + characterList.Count) % characterList.Count;

        _character = characterList[nextIndex];

        // 이후 처리 (UI 업데이트 등)
        SetInfo(_character);

    }

    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
        Clear();
    }



    public void SetInfo(Character character)
    {
        _character = character;
        //GetImage((int)Images.Image).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        GetText((int)Texts.CharacterNameText).text = _character.Data.Name;

        //foreach(string quip in _character.EquippedItemIds)
        //{
        //    UI_EquipItem item = Managers.UI.MakeSubItem<UI_EquipItem>(GetObject((int)GameObjects.EquippedGroupObject).transform);
        //    item.SetInfo(quip);
        //}

        //Todo : 모자, 가방, 악세서리 아이템 나오면 연결 MakeSubItem말고 고정값들

        Refresh();
    }
    private void Refresh()
    {
        if (_character == null)
            return;

        if(_characterAI != null)
        {
            Clear();
        }

        _equipHatItem.SetInfo(_character); 
        _equipAccessoryItem.SetInfo(_character);
        _equipBagItem.SetInfo(_character);

        _characterAI = Managers.Object.Spawn<AICharacter>(new Vector3(500, 500, 500), _character.DataId, null, true);
    }


    private void Clear()
    {
        Managers.Resource.Destroy(_characterAI.gameObject);
        _characterAI = null;
    }
}
