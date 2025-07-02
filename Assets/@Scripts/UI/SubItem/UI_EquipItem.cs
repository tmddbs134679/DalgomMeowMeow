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

    Character _character;
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

        return true;
    }

    public void SetInfo(Character character)
    {
       // Todo : 아이템에서 리스트 뽑기
       _character = character;

        if(_character.EquippedItemIds.Count == 0)
        {
            GetText((int)Texts.CostumeText).gameObject.SetActive(true);
            GetText((int)Texts.NameText).gameObject.SetActive(false);
            GetText((int)Texts.DescriptionText).gameObject.SetActive(false);
        }
        else
        {
            //Todo : 보유 장비 나열
        }


    }
}
