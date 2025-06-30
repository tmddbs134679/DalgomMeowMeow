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
    public float Gold = 0;

    //public List <캐릭터들>
    public List<Character> Characters = new List<Character>();

}


public class GameManager 
{
    public GameData _gameData = new GameData();

    public bool IsLoaded = false;

    public int CurrentStage;
    public bool CurrentStageCleared;
    #region Action

    public event Action OnResourcesChagned;
    public event Action OnCharacterChanged;
    #endregion

    #region GameData

    public List<Character> Characters
    {
        get { return _gameData.Characters; }
        set
        {
            _gameData.Characters = value;

        }
    }
    public float Gold
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
        {
            foreach (Character ch in _gameData.Characters)
            {
                if (Managers.Data.CreatureDic.TryGetValue(ch.DataId, out var creatureData))
                    ch.SetInfo(creatureData);
            }
            return;
        }

        var newChar = new Character();
        newChar.Init("C0001", new Vector3(0, 0, 0)); // 위치 000, IDLE 상태
        newChar.SetInfo(Managers.Data.CreatureDic["A10001"]); // CreatureData 연결

        _gameData.Characters.Add(newChar);
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
