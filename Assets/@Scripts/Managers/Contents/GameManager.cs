using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using static Define;

[Serializable]
public class GameData
{
    public float Gold = 0;

    //public List <캐릭터들>
   
    // 저장 전용
    public List<Character> CharacterList = new List<Character>();
    public List<Equipment> OwnedEquipments = new List<Equipment>();
}


public class GameManager 
{
    public GameData _gameData = new GameData();


    private Dictionary<string, Character> _characters = new Dictionary<string, Character>();

    public bool IsLoaded = false;

    public int CurrentStage;
    public bool CurrentStageCleared;
    #region Action

    public event Action OnResourcesChagned;
    public event Action OnCharacterChanged;
    public event Action EquipInfoChanged;
    #endregion

    #region GameData

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
    public List<Character> Characters
    {
        get { return _gameData.CharacterList; }
        set
        {
            _gameData.CharacterList = value;
            OnCharacterChanged?.Invoke();
        }
    }

    public List<Equipment> OwnedEquipments
    {
        get { return _gameData.OwnedEquipments; }
        set
        {
            _gameData.OwnedEquipments = value;

            EquipInfoChanged?.Invoke();
        }
    }
    #endregion




    #region Save

    string _path;





    public void Init()
    {
        _path = Application.persistentDataPath + "/SaveData.json";

        if (LoadGame())
        {
            // List → Dictionary 변환
            _characters.Clear();
            foreach (Character character in _gameData.CharacterList)
            {
                if (Managers.Data.CreatureDic.TryGetValue(character.DataId, out var creatureData))
                    character.SetInfo(creatureData);

                _characters[character.DataId] = character;
            }

            return;
        }




        // 최초 생성
        var newChar = new Character();
        newChar.Init("A10001", new Vector3(10f, 0, 8f)); // 위치 초기값
        newChar.SetInfo(Managers.Data.CreatureDic["A10001"]);
        _characters[newChar.Id] = newChar;

        newChar.Init("A10002", new Vector3(10f, 0, 8f)); // 위치 초기값
        newChar.SetInfo(Managers.Data.CreatureDic["A10002"]);
        _characters[newChar.Id] = newChar;

        // 초기 장비 생성
        Equipment hat = new Equipment("E0001");
        Equipment accessory = new Equipment("E0002");
        Equipment bag = new Equipment("E0003");

        OwnedEquipments.Add(hat);
        OwnedEquipments.Add(accessory);
        OwnedEquipments.Add(bag);

        SaveGame();
        IsLoaded = true;
    }



    public void SaveGame()
    {

        UpdateCharactersFromWorld();

        _gameData.CharacterList = new List<Character>(_characters.Values);

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

    public void UpdateCharactersFromWorld()
    {
        foreach (var pair in _characters)
        {
            Character character = pair.Value;

            AICharacter ai = Util.FindAIById(character.DataId);
            if (ai == null) continue;

            character.Pos = new Vector3Data(ai.transform.position);
            character.CurrentState = ai.CharacterData.CurrentState;
            character.CurrentStamina = ai.CharacterData.CurrentStamina;
        }
    }

    #endregion

    #region Equipment

    public void EquipItem(string characterId, EEquipmentType type, Equipment equipment)
    {
        if(_characters.TryGetValue(characterId, out var character) == false)
        {
            Debug.LogError("못찾음");
            return;
        }

        if(character.EquippedItems.TryGetValue(type, out Equipment prevEquip))
        {
            prevEquip.IsEquipped = false;
        }

        character.EquippedItems[type] = equipment;
        equipment.IsEquipped = true;

        SaveGame();
        OnCharacterChanged?.Invoke();
    }

    


    #endregion



}
