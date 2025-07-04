using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System;

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

                soldFood.FoodData.Price = Mathf.FloorToInt(soldFood.FoodData.Price * 0.5f);
                Managers.Game.Gold += soldFood.FoodData.Price;
               
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


    public void MakeFood()
    {
        Food food = new Food("F0001");
        Enqueue(food);
    }

    // TODO : cook된거 데이터를 받아오던 랜덤을 받아오던 여기서 처리 
}
