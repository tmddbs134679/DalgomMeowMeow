using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SaveTest : MonoBehaviour
{

    GameManager _game;
    // Start is called before the first frame update
    void Start()
    {
        _game = Managers.Game;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveButton()
    {
        _game.SaveGame();
    }
}
