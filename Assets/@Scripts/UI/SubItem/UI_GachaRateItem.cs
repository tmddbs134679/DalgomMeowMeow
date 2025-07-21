using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GachaRateItem : UI_Base
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
        ItemValueText,
        ItemRateValueText
    }
    #endregion

    Define.EGachaType _type;

    private void OnEnable()
    {


    }
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        return true;
    }


    public void SetInfo(string equipName, EEquipmentGrade type)
    {
        float rate = COMMON_GACHA_GRADE[(int)type];

        GetText((int)Texts.ItemValueText).text = equipName;
        GetText((int)Texts.ItemRateValueText).text = rate.ToString();
    }

    public void SetInfo(string equipName, float probability)
    {
        GetText((int)Texts.ItemValueText).text = equipName;
        GetText((int)Texts.ItemRateValueText).text = probability.ToString();
    }
}
