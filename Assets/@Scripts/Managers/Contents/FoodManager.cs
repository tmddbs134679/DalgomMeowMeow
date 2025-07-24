using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System;
using System.Linq;

public class FoodManager
{
    
    public LinkedList<Food> _foodList = new LinkedList<Food>();
    private Dictionary<Food, LinkedListNode<Food>> _nodeMap = new Dictionary<Food, LinkedListNode<Food>>();

    #region Action

    public event Action<Food> OnFoodAdded;
    public event Action<Food> OnFoodSold;

    #endregion

    public void Enqueue(Food food)
    {
        var node = _foodList.AddLast(food);
        _nodeMap[food] = node;

        OnFoodAdded?.Invoke(food);


        if (_foodList.Count > Define.FOOD_MAX_VALUE)
        {
            var first = _foodList.First;
            if (first != null)
            {
                Food soldFood = first.Value;
                Cancel(soldFood);
                OnFoodSold?.Invoke(soldFood);
                int discountPrice = Mathf.FloorToInt(soldFood.FoodData.Price * 0.5f);
                Managers.Game.Gold += discountPrice;
            }
        }
    }

    public void Cancel(Food food)
    {
        if (_nodeMap.TryGetValue(food, out var node))
        {

            _foodList.Remove(node);
            _nodeMap.Remove(food);

            OnFoodSold?.Invoke(food);
        }
    }


    public void MakeFood(IngredientSet ingredients, int buildingLevel)
    {
        FoodData recipe = Managers.Data.FoodDic.Values
                 .FirstOrDefault(fd =>
                     fd.Cabbage == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Cabbage) &&
                     fd.Carrot == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Carrot) &&
                     fd.Pumpkin == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Pumpkin) &&
                     fd.Potato == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Potato) &&
                     fd.Onion == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Onion));

        //요리가 없으면 기본요리 생산
        if (recipe == null)
        {
            recipe = Managers.Data.FoodDic["F0001"];
        }

        // 가격 계산
        float basePrice = recipe.Price;
        float ingredientBonus = 0.1f; // 재료 1개당 +10%
        float fieldMultiplier = ingredients.GetAvgFieldLevelMultiplier();
        float cookingMultiplier = 1.0f + ((buildingLevel - 1) * 0.15f); // Lv1=1.0, Lv2=1.15, Lv3=1.3

        float finalPrice = basePrice * (1 + ingredients.TotalCount * ingredientBonus)
                                     * fieldMultiplier
                                     * cookingMultiplier;

        recipe.Price = Mathf.FloorToInt(finalPrice);

        // 요리 생성
        Food food = new Food(recipe);
        Enqueue(food);
    }


}
