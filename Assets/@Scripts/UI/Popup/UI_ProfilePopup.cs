
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using static Define;


public class UI_ProfilePopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        ContentObject,
        EquippedObject,
        EquippedGroupObject,
        InformationObject
    }

    enum Buttons
    {
        NextCharacterButton,
        PrevCharacterButton,
        ExitButton,
    }

    enum Texts
    {
        CharacterNameText,
        LevelText,
        HPText,
        AtkText,
        SpeedText,
        MaxStaminaText,
        SkillText
    }

    enum Images
    {
        SkillImage
    }

    enum InputFields
    {
        InputFieldText,
    }

    enum Toggles
    {
        EquipToggle,
        InfoToggle
    }

    #endregion

    Character _character;
    AICharacter _characterAI;

    List<EEquipmentType> displayOrder = new()
    {
        EEquipmentType.Hat,
        EEquipmentType.Accessory,
        EEquipmentType.Bag
    };

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        GetObject((int)GameObjects.InformationObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.EquippedObject).gameObject.SetActive(true);

        GetToggle((int)Toggles.EquipToggle).isOn = true;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindToggle(typeof(Toggles));
        BindInputField(typeof(InputFields));


        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.PrevCharacterButton).gameObject.BindEvent(() => OnClickChangeButton(1));
        GetButton((int)Buttons.NextCharacterButton).gameObject.BindEvent(() => OnClickChangeButton(-1));
        GetInputField((int)InputFields.InputFieldText).onEndEdit.AddListener(OnInputFiled);

        GetToggle((int)Toggles.EquipToggle).gameObject.BindEvent(OnEquipToggleButton);
        GetToggle((int)Toggles.InfoToggle).gameObject.BindEvent(OnInfoToggleButton);

 

       GetText((int)Texts.CharacterNameText).gameObject.BindEvent(OnClickName);
        GetInputField((int)InputFields.InputFieldText).gameObject.SetActive(false);
        _character = Managers.Game.Characters[0];

        //Refresh();

        return true;
    }

    private void OnInfoToggleButton()
    {
        GetObject((int)GameObjects.InformationObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.EquippedObject).gameObject.SetActive(false);
    }

    private void OnEquipToggleButton()
    {
        GetObject((int)GameObjects.InformationObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.EquippedObject).gameObject.SetActive(true);
    }

    private void OnInputFiled(string name)
    {
        Character targetCharacter = Managers.Game.Characters.Find(c => c.UniqueId == _character.UniqueId);

        if (targetCharacter != null)
        {
            targetCharacter.Name = name; // 이름 변경
            GetInputField((int)InputFields.InputFieldText).gameObject.SetActive(false);
            GetText((int)Texts.CharacterNameText).gameObject.SetActive(true);
            GetText((int)Texts.CharacterNameText).text = targetCharacter.Name;

            Managers.UI.OnCharacterChange?.Invoke(targetCharacter);
        }
    } 
    
    private void OnClickName()
    {
        GetText((int)Texts.CharacterNameText).gameObject.SetActive(false);

        GetInputField((int)InputFields.InputFieldText).gameObject.SetActive(true);
        GetInputField((int)InputFields.InputFieldText).text = "";
        GetInputField((int)InputFields.InputFieldText).ActivateInputField();
        string newName = GetInputField((int)InputFields.InputFieldText).text.Trim();
        if (string.IsNullOrEmpty(newName)) return;

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

        GetObject((int)GameObjects.EquippedGroupObject).DestroyChilds();


        _character = character;
        GetText((int)Texts.CharacterNameText).text = _character.Name;


        foreach (EEquipmentType type in displayOrder)
        {
            // 해당 타입의 장비를 찾아서 UI 생성

            UI_EquipItem item = Managers.UI.MakeSubItem<UI_EquipItem>(GetObject((int)GameObjects.EquippedGroupObject).transform);
            item.transform.SetAsLastSibling();

            if (_character.EquippedItems.TryGetValue(type, out var equip))
                item.SetInfo(equip.UniqueId);
            else
                item.SetInfo();
        }


        GetText((int)Texts.LevelText).text = _character.Level.ToString();
        GetText((int)Texts.HPText).text = _character.Hp.ToString();
        GetText((int)Texts.AtkText).text = _character.Atk.ToString();
        GetText((int)Texts.SpeedText).text = _character.MoveSpeed.ToString();
        GetText((int)Texts.MaxStaminaText).text = _character.MaxStamina.ToString();
        GetText((int)Texts.SkillText).text = Managers.Data.SkillDataDic[_character.Data.SkillID].Description;
        GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(character.Data.SkillID.ToString());

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
        _characterAI = Managers.Object.Spawn<AICharacter>(new Vector3(500, 500, 500), _character.DataId, null, true);
        _characterAI.ReplicaSetting(_character);
       
    }


    private void Clear()
    {
        Managers.Resource.Destroy(_characterAI.gameObject);
        _characterAI = null;
    }
}
