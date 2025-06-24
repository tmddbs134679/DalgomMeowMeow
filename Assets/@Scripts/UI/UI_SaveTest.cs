using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveTest : MonoBehaviour
{

    GameManager _game;

    public Button plus;
    public Button minus;   
    public  TextMeshProUGUI _textMeshPro;
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
}
