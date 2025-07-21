using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_GachaTablePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        GachaInfoContentObject
    }

    enum Buttons
    {
        BackgroundButton
    }

    enum Texts
    {

    }
    #endregion

    Define.EGachaType _type;

    private void OnEnable()
    {
        Refresh();
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject).gameObject);
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


        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButon);

 
        return true;
    }


    public void SetInfo(Define.EGachaType type)
    {
        _type = type;
    }
    private void OnClickBackgroundButon()
    {   
        gameObject.SetActive(false);
    }


    private void Refresh()
    {

        GetObject((int)GameObjects.GachaInfoContentObject).DestroyChilds();


        if (_type == Define.EGachaType.Creature)
        {
            foreach (GachaData data in Managers.Data.GachaDic.Values)
            {
                UI_GachaRateItem Item = Managers.UI.MakeSubItem<UI_GachaRateItem>(GetObject((int)GameObjects.GachaInfoContentObject).transform);
                Item.SetInfo(Managers.Data.CreatureDic[data.DataId].Name, data.Probability);
            }
        }
        else if(_type == Define.EGachaType.Equipment)
        {
            foreach(EquipmentGachaData data in Managers.Data.GachaTableDataDic.Values)
            {
                UI_GachaRateItem Item = Managers.UI.MakeSubItem<UI_GachaRateItem>(GetObject((int)GameObjects.GachaInfoContentObject).transform);
                Item.SetInfo(Managers.Data.EquipmentDic[data.EquipmentID].Name, data.Grade);
            }

           

        }
    }

}
