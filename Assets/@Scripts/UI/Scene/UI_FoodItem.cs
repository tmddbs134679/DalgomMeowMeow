using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public FoodData _food;
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

        return true;
    }

    public void SetInfo(FoodData food)
    {
        _food = food;

        GetImage((int)Images.FoodImage).sprite = _food.Icon;
        GetText((int)Texts.FoodPriceText).text = _food.Price.ToString();

    }



    void OnClickFoodItemButton()
    {
       //누르면 GameManager의 돈으로 
    }
}
