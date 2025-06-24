using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


[Serializable]
public class GameData
{
    public int Gold = 0;

    //public List <캐릭터들>
    public List<BuildingBase> Buildings = new List<BuildingBase>();
}


public class GameManager 
{
    public GameData _gameData = new GameData();

    #region Action

    public event Action OnResourcesChagned;

    #endregion

    #region GameData

    public List<BuildingBase> Buildings
    {
        get { return _gameData.Buildings;}
        set
        {
            _gameData.Buildings = value;
        }
    }

    public int Gold
    {
        get { return _gameData.Gold; }
        set
        {
            _gameData.Gold = value;
            SaveGame();
            OnResourcesChagned?.Invoke();
        }
    }

    #endregion


    #region Save

    string _path;


    #endregion

    public void Init()
    {
        _path = Application.persistentDataPath + "/SaveData.json";
    }

    public void SaveGame()
    {
        string jsonStr = JsonConvert.SerializeObject(_gameData);
        File.WriteAllText(_path, jsonStr);
    }
}
