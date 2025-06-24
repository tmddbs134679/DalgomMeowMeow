using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData")]

public class IngredientData : ScriptableObject
{
    public Define.EItemType ItemType; // 재료의 종류 (채소, 생선 등)
    public int ItemID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Price;
    
}

