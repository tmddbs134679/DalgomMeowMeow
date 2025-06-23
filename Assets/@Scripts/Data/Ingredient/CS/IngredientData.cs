using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData")]

public class IngredientData : ScriptableObject
{
    public int ItemID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Price;
}
