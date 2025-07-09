using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipItem : UI_Base
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
        NameText,
        DescriptionText,
        CostumeText,
    }

    enum Images
    {
        Image
    }
    #endregion

    //Character _character;

    Equipment _equipment;
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
        return true;
    }

    public void SetInfo(string equipUid = null)
    {
        // Todo : 아이템에서 리스트 뽑기
        // _character = character;
        _equipment = Managers.Game.OwnedEquipments.Find(e => e.UniqueId == equipUid);

        bool hasEquip = _equipment != null;

        if (_equipment != null)
        {
            GetImage((int)Images.Image).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
            GetText((int)Texts.NameText).text = _equipment.EquipmentData.Name;
            GetText((int)Texts.DescriptionText).text = _equipment.EquipmentData.Description;
        }
        else
        {
            GetImage((int)Images.Image).sprite = null;
        }

        GetText((int)Texts.CostumeText).gameObject.SetActive(!hasEquip);
        GetText((int)Texts.NameText).gameObject.SetActive(hasEquip);
        GetText((int)Texts.DescriptionText).gameObject.SetActive(hasEquip);


    }
}
