using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;
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
        newChar.Init("A2002", new Vector3(39f, 0, 27f)); // 위치 초기값
        newChar.SetInfo(Managers.Data.CreatureDic["A20002"]);
        _characters[newChar.Id] = newChar;

        var newChar1 = new Character();
        newChar1.Init("A20006", new Vector3(38f, 0, 27f)); // 위치 초기값
        newChar1.SetInfo(Managers.Data.CreatureDic["A20006"]);
        _characters[newChar1.Id] = newChar1;

        var newChar2 = new Character();
        newChar2.Init("A10003", new Vector3(38f, 0, 27f)); // 위치 초기값
        newChar2.SetInfo(Managers.Data.CreatureDic["A10003"]);
        _characters[newChar2.Id] = newChar2;

        var newChar3 = new Character();
        newChar3.Init("A20001", new Vector3(38f, 0, 27f)); // 위치 초기값
        newChar3.SetInfo(Managers.Data.CreatureDic["A20001"]);
        _characters[newChar3.Id] = newChar3;



        // 초기 장비 생성
        Equipment hat = new Equipment("E0001");
        Equipment hat2 = new Equipment("E0004");
        Equipment hat3 = new Equipment("E0003");
        Equipment ac1 = new Equipment("E0101");
        Equipment ac2 = new Equipment("E0102");
        Equipment bag1 = new Equipment("E0201");
        // Equipment bag2 = new Equipment("E0204");
        OwnedEquipments.Add(hat);
        OwnedEquipments.Add(hat2);
        OwnedEquipments.Add(hat3);
        OwnedEquipments.Add(ac1);
        OwnedEquipments.Add(ac2);
        OwnedEquipments.Add(bag1);
        //OwnedEquipments.Add(bag2);

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

    //public void EquipItem(string characterId, EEquipmentType type, Equipment equipment)
    //{
    //    if (_characters.TryGetValue(characterId, out var character) == false)
    //    {
    //        Debug.LogError("못찾음");
    //        return;
    //    }

    //    if (character.EquippedItems.TryGetValue(type, out Equipment prevEquip))
    //    {
    //        prevEquip.IsEquipped = false;
    //    }

    //    character.EquippedItems[type] = equipment;
    //    equipment.IsEquipped = true;

    //    SaveGame();
    //    OnCharacterChanged?.Invoke();
    //}

    private void EquipCharacterVisual(AICharacter ai, Character character)
    {
        foreach (var equipId in character.EquippedItemIds)
        {
            Equipment equip = OwnedEquipments.Find(e => e.key == equipId);
            if (equip == null)
            {
                Debug.LogWarning($"장비 ID {equipId}를 찾을 수 없음");
                continue;
            }

            // View용 오브젝트 장착 시각화
            AttachEquipmentToCharacter(ai, equip);

            // 캐릭터 데이터 딕셔너리도 구성
            if (!character.EquippedItems.ContainsKey(equip.EquipmentData.EquipmentType))
                character.EquippedItems[equip.EquipmentData.EquipmentType] = equip;
        }
    }

    private void AttachEquipmentToCharacter(AICharacter ai, Equipment equipment)
    {
        var type = equipment.EquipmentData.EquipmentType;

        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"장비 본이 존재하지 않음: {type}");
            return;
        }

        // 기존 장비 제거
        Transform old = bone.Find("Equipped_" + type);
        if (old != null)
            GameObject.Destroy(old.gameObject);

       // GameObject go = GameObject.Instantiate(equipment.EquipmentData.Prefab, bone);
      //  go.name = "Equipped_" + type;
    }
    public void SetInitEquipment(AICharacter character)
    {
        foreach (var equipId in character.EquippedItemIds)
        {
            Equipment equip = OwnedEquipments.Find(e => e.key == equipId);
            if (equip == null)
            {
                Debug.LogWarning($"장착 장비 {equipId} 를 못 찾음");
                continue;
            }

            // Dictionary에도 넣어줌
            if (!character.CharacterData.EquippedItems.ContainsKey(equip.EquipmentData.EquipmentType))
                character.CharacterData.EquippedItems.Add(equip.EquipmentData.EquipmentType, equip);

            AttachEquipmentToCharacter(character, equip);
        }
    }

    #endregion

    #region Gacha

    public string DrawRandomCreature()
    {
        var creatureDic = Managers.Data.CreatureDic;

        var pairList = new List<KeyValuePair<string, Data.CreatureData>>(creatureDic);

        int randIndex = UnityEngine.Random.Range(0, 11);

        var randomPair = pairList[randIndex];


        return randomPair.Key;
    }

    public AICharacter SpawnRandomGachaCharacter(Vector3 spawnPos)
    {
        string creatureId = DrawRandomCreature();

        if (!Managers.Data.CreatureDic.TryGetValue(creatureId, out var creatureData))
        {
            return null;
        }

        Character newChar = new Character();
        newChar.SetInfo(creatureData);
        newChar.Init(creatureId, spawnPos);

        AICharacter aiChar = Managers.Object.Spawn<AICharacter>(spawnPos, creatureId, isReplica: false);

        if (aiChar == null)
        {
            return null;
        }

        aiChar.SetInfo(newChar);
        Init();

        return aiChar;
    }


    #endregion




}

