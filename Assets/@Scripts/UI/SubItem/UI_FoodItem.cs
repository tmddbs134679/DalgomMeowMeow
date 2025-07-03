using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Data;
public class UI_FoodItem : UI_Base
{
    #region Enum

    enum Texts
    {
        FoodPriceText,
    }

    enum Images
    {
        FoodImage,

    }
    #endregion

    public Action OnClickFoodItem;
    public Food _food;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        gameObject.BindEvent(OnClickFoodItemButton);
        gameObject.GetOrAddComponent<UI_ButtonAnimation>(); 
        return true;
    }

    public void SetInfo(Food food)
    {
   
        _food = food;

        Sprite spr = Managers.Resource.Load<Sprite>(_food.FoodData.SpriteName);
        GetImage((int)Images.FoodImage).sprite = spr;
        GetText((int)Texts.FoodPriceText).text = _food.FoodData.Price.ToString();
       
    }



    void OnClickFoodItemButton()
    {
        Debug.Log(name);
        //누르면 GameManager의 돈으로 바꿈.
        Managers.Game.Gold += _food.FoodData.Price;
        Managers.Food.Cancel(_food);

        (Managers.UI.SceneUI as UI_GameScene).RemoveSlotAnimated(this);
    }
}
