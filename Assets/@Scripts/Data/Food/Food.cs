using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class Food
{
    public string key = "";

    public Data.FoodData FoodData;
    public Food(string key)
    {
        this.key = key;
        FoodData = Managers.Data.FoodDic[key];  

    }
}
