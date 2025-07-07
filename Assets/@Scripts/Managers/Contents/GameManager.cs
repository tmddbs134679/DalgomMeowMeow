using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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

    public Dictionary<string, Character> CharacterMap = new();
    // 씬에 존재하는 실제 캐릭터 오브젝트
    public Dictionary<string, AICharacter> CharactersInScene = new();

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

        //var newChar1 = new Character();
        //newChar1.Init("A20006", new Vector3(38f, 0, 27f)); // 위치 초기값
        //newChar1.SetInfo(Managers.Data.CreatureDic["A20006"]);
        //_characters[newChar1.Id] = newChar1;

        //var newChar2 = new Character();
        //newChar2.Init("A10003", new Vector3(38f, 0, 27f)); // 위치 초기값
        //newChar2.SetInfo(Managers.Data.CreatureDic["A10003"]);
        //_characters[newChar2.Id] = newChar2;

        //var newChar3 = new Character();
        //newChar3.Init("A20001", new Vector3(38f, 0, 27f)); // 위치 초기값
        //newChar3.SetInfo(Managers.Data.CreatureDic["A20001"]);
        //_characters[newChar3.Id] = newChar3;



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
        {
            _gameData = data;
            CharacterMap = _gameData.CharacterList.ToDictionary(c => c.Id, c => c);
        }
          

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

    public void EquipCharacterVisual(AICharacter ai, Character character, Equipment previewEquipment = null)
    {
        // 기존 장비 장착 (진짜 착용 정보 기반)
        foreach (var equipId in character.EquippedItemIds)
        {
            Equipment equip = OwnedEquipments.Find(e => e.key == equipId);
            if (equip == null) continue;

            AttachEquipmentToCharacter(ai, equip);
        }

        // 선택된 미리보기 장비 장착
        if (previewEquipment != null)
        {
           // AttachPreviewToCharacter(ai, previewEquipment);
        }
    }

    public void EquipmentItem(Character character, Equipment equipment)
    {

        var id = character.Id;
        var targetCharacter = CharacterMap.ContainsKey(id) ? CharacterMap[id] : Characters.Find(c => c.Id == id);
        if (targetCharacter == null) return;

        // 1. 데이터 반영
        var type = equipment.EquipmentData.EquipmentType;
        targetCharacter.EquippedItems[type] = equipment;
        if (!targetCharacter.EquippedItemIds.Contains(equipment.key))
            targetCharacter.EquippedItemIds.Add(equipment.key);

        // 2. 시각화
        if (CharactersInScene.TryGetValue(id, out var ai))
            AttachEquipmentToCharacter(ai, equipment);

        equipment.EquippedByCharacterId = targetCharacter.DataId;
        equipment.IsEquipped = true;

        // 3. 이벤트/저장
        OnCharacterChanged?.Invoke();
        EquipInfoChanged?.Invoke();
        SaveGame();
    }

    public void SetInitEquipment(AICharacter character)
    {
        foreach (var equipId in character.CharacterData.EquippedItemIds)
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

            EquipmentItem(character.CharacterData, equip);
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
        EquipmentController go = Managers.Object.Spawn<EquipmentController>(Vector3.zero, equipment.EquipmentData.DataId, bone);
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

