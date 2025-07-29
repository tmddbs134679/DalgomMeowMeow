using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class Food
{
    public string key = "";

    public Data.FoodData FoodData;
    public int CalculatedPrice;
    public Food(FoodData data, int calculatedPrice)
    {
        FoodData = data;
        CalculatedPrice = calculatedPrice;
    }
}
