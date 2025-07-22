using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_EquipmentInfoPopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        ContentObject,
        EquippedObject
    }

    enum Buttons
    {
        BackgroundButton,
        EquipButton
    }

    enum Texts
    {
        EquipmentText,
        EquipmentDescriptionText,
        EquipButonText
    }

    enum Images
    {
        EquipmentImage,
        EquippedCharacterImage,
        UnEquipCharacterImage

    }
    #endregion

    Equipment _equipment;
    private const string _equipText = "착용하기";
    private const string _equipChangeText = "교체하기";
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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(() => OnCilckEquipButton());
        GetButton((int)Buttons.EquipButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        return true;
    }


    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        Managers.Equipment.EquipInfoChanged += Refresh;
    }

    private void OnDestroy()
    {
        Managers.Equipment.EquipInfoChanged -= Refresh;
    }
    private void OnCilckEquipButton()
    {
        UI_EquipmentTypePopup popUp = Managers.UI.ShowPopupUI<UI_EquipmentTypePopup>();
         popUp.SetInfo(_equipment);
        

    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;

        // 캐릭터가 착용하지 않은 장비
        if (_equipment.EquippedByCharacterId != null)  
            SetEquipUIActive(true);
        else   //착용한 장비
            SetEquipUIActive(false);


        GetText((int)Texts.EquipmentText).text = _equipment.EquipmentData.Name;
        GetText((int)Texts.EquipmentDescriptionText).text = _equipment.EquipmentData.Description;
        GetImage((int)Images.EquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);

    }
    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void SetEquipUIActive(bool isEquipped)
    {
        // 장비가 착용된 상태
        GetObject((int)GameObjects.EquippedObject).gameObject.SetActive(isEquipped);
        GetImage((int)Images.UnEquipCharacterImage).gameObject.SetActive(!isEquipped);
        GetImage((int)Images.EquippedCharacterImage).gameObject.SetActive(isEquipped);

        if (isEquipped)
        {
            Character character = Managers.Game.Characters.Find(c => c.UniqueId == _equipment.EquippedByCharacterId);
            string spriteName = character.Data.IconLabel;

            GetImage((int)Images.EquippedCharacterImage).sprite = Managers.Resource.Load<Sprite>(spriteName);
            GetText((int)Texts.EquipButonText).text = _equipChangeText;
            
        }
        else
        {
            GetText((int)Texts.EquipButonText).text = _equipText;
        }
    }

    private void Refresh()
    {
        //GetImage((int)Images.EquippedCharacterImage).sprite = null;
        //SetEquipUIActive(false);
        SetInfo(_equipment);

    }
}
