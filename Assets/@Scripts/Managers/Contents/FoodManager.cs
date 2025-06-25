using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
public class FoodManager
{
    
    //public GameObject[] ServeSlot;
    //private Image[] _serveSlotImages;
    private Queue<Food> _foodQueue = new Queue<Food>();

    //private void Awake()
    //{
    //    SetComponent();
    //}

    //private void SetComponent()
    //{
    //    _serveSlotImages = new Image[ServeSlot.Length];
    //    for (int i = 0; i < ServeSlot.Length; i++)
    //    {
    //        _serveSlotImages[i] = ServeSlot[i].GetComponent<Image>();
    //    }
    //}

    public void Enqueue(Food food)
    {
        _foodQueue.Enqueue(food);



       // SetSprite();
    }

    public Food Dequeue()
    {
        if (_foodQueue.Count == 0)
        {
            Debug.Log("Empty!");
            return null;
        }
        var food = _foodQueue.Dequeue();
       // SetSprite();
        return food;
    }

    
    //public void SetSprite()
    //{
    //    int index = 0;
    //    foreach(var food in _foodQueue)
    //    {
    //        if(index >= ServeSlot.Length)
    //            break;

    //        _serveSlotImages[index].sprite = food.Icon;
    //        ServeSlot[index].SetActive(true);
    //        index++;
    //    }

    //    for (; index < ServeSlot.Length; index++)
    //    {
    //        ServeSlot[index].SetActive(false);
    //    }
    //}

    //[ContextMenu("큐 확인")]
    //public void QueueCheck()
    //{
        
    //}
    //public void SlotOff()
    //{
    //    foreach (var slot in ServeSlot)
    //    {
    //        slot.SetActive(false);
    //    }
    //}

    //public void Clear()
    //{
    //    _foodQueue.Clear();
    //    SlotOff();
    //}
}
