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
                int discountPrice = Mathf.FloorToInt(soldFood.CalculatedPrice * 0.5f);
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
        // 음식 레시피 가져오기
        FoodData recipe = Managers.Data.FoodDic.Values
            .FirstOrDefault(fd =>
                fd.Cabbage == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Cabbage) &&
                fd.Carrot == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Carrot) &&
                fd.Pumpkin == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Pumpkin) &&
                fd.Potato == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Potato) &&
                fd.Onion == ingredients.Ingredients.Exists(i => i.Type == Define.ECropType.Onion));

        // 요리가 없으면 기본 요리로 설정
        if (recipe == null)
        {
            recipe = Managers.Data.FoodDic["F0001"];
        }

        // 기본 가격
        float basePrice = recipe.Price;

        // 재료 1개당 +10% 추가 보너스
        float ingredientBonus = 0.1f;

        // 밭 레벨에 따른 곱셈
        float fieldMultiplier = ingredients.GetAvgFieldLevelMultiplier();

        // 건물 레벨에 따른 곱셈 (레벨업 시에만 적용)
        float cookingMultiplier = 1.0f + ((buildingLevel - 1) * 0.15f); // 건물 레벨이 증가할 때마다 가격이 증가

        // 최종 가격 계산
        float finalPrice = basePrice * (1 + ingredients.TotalCount * ingredientBonus)
                                         * fieldMultiplier
                                         * cookingMultiplier;

        // 여기서는 recipe.Price를 덮어쓰지 않고, 최종 계산된 가격을 그냥 사용함
        int calculatedPrice = Mathf.FloorToInt(finalPrice);

        // 요리 생성
        Food food = new Food(recipe, calculatedPrice);

        Enqueue(food);
    }


}
