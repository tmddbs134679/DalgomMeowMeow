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
    public List<AICharacter> Characters = new List<AICharacter>();  
}


public class GameManager 
{
    public GameData _gameData = new GameData();

    public bool IsLoaded = false;
    #region Action

    public event Action OnResourcesChagned;
    public event Action OnCharacterChanged;
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

    public List<AICharacter> Characters
    {
        get { return _gameData.Characters; }
        set
        {
            _gameData.Characters = value;
            OnCharacterChanged?.Invoke();
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

        if (LoadGame())
            return;

        IsLoaded = true;

        SaveGame();

    }



    public void SaveGame()
    {
        string jsonStr = JsonConvert.SerializeObject(_gameData);
        File.WriteAllText(_path, jsonStr);
    }
    private bool LoadGame()
    {
   
        if (File.Exists(_path) == false)
            return false;


        string fileStr = File.ReadAllText(_path);
        GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
        if (data != null)
            _gameData = data;

        IsLoaded = true;
        return true;
    }


}
