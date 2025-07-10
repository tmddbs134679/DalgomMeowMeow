using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class Food
{
    public string key = "";

    public Data.FoodData FoodData;
    public Food(FoodData data)
    {
        FoodData = data; 

    }
}
