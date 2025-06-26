using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveTest : MonoBehaviour
{

    GameManager _game;

    public GameObject storageobj;

    // Start is called before the first frame update
    void Start()
    {
        _game = Managers.Game;
        _textMeshPro.text = _game.Gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Cook

    public Button CookButton;

    public void Cook()
    {
        Debug.Log("Cook");
        Food food = new Food("F0001");
        Managers.Food.Enqueue(food);

        UI_FoodItem item = Managers.UI.MakeSubItem<UI_FoodItem>(storageobj.transform);
        item.SetInfo(food);

    }


    #endregion
    #region SaveTest

    public Button plus;
    public Button minus;
    public TextMeshProUGUI _textMeshPro;
    public void SaveButton()
    {
        _game.SaveGame();
    }

    public void Plus()
    {
        _game.Gold++;
    }

    public void Minus()
    {
        _game.Gold--;
    }

    #endregion
}
