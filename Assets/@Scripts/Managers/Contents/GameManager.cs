using Data;
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


    public bool[] AttendanceReceived = new bool[30];

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

    public bool[] AttendanceReceived
    {
        get { return _gameData.AttendanceReceived; }
        set { _gameData.AttendanceReceived = value; }

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
        newChar.Init("A10002", new Vector3(39f, 0, 27f)); // 위치 초기값
        newChar.SetInfo(Managers.Data.CreatureDic["A10002"]);
        _characters[newChar.Id] = newChar;

        var newChar1 = new Character();
        newChar1.Init("A10006", new Vector3(38f, 0, 27f)); // 위치 초기값
        newChar1.SetInfo(Managers.Data.CreatureDic["A10006"]);
        _characters[newChar1.Id] = newChar1;

        var newChar2 = new Character();
        newChar2.Init("A10003", new Vector3(38f, 0, 30f)); // 위치 초기값
        newChar2.SetInfo(Managers.Data.CreatureDic["A10003"]);
        _characters[newChar2.Id] = newChar2;

        var newChar3 = new Character();
        newChar3.Init("A10001", new Vector3(38f, 0, 30f)); // 위치 초기값
        newChar3.SetInfo(Managers.Data.CreatureDic["A10001"]);
        _characters[newChar3.Id] = newChar3;



        // 초기 장비 생성
        Equipment eq1 = new Equipment("E0001");
        Equipment eq2 = new Equipment("E0002");
        Equipment eq3 = new Equipment("E0003");
        Equipment eq4 = new Equipment("E0004");
        Equipment eq5 = new Equipment("E0005");
        Equipment eq6 = new Equipment("E0006");
        Equipment eq7 = new Equipment("E0007");
        Equipment eq8 = new Equipment("E0101");
        Equipment eq9 = new Equipment("E0102");
        Equipment eq10 = new Equipment("E0201");
        Equipment eq11 = new Equipment("E0202");
        Equipment eq12 = new Equipment("E0203");
        Equipment eq13 = new Equipment("E0205");
        Equipment eq14 = new Equipment("E0207");
        OwnedEquipments.Add(eq1);
        OwnedEquipments.Add(eq2);
        OwnedEquipments.Add(eq3);
        OwnedEquipments.Add(eq4);
        OwnedEquipments.Add(eq5);
        OwnedEquipments.Add(eq6);
        OwnedEquipments.Add(eq7);
        OwnedEquipments.Add(eq8);
        OwnedEquipments.Add(eq9);
        OwnedEquipments.Add(eq10);
        OwnedEquipments.Add(eq11);
        OwnedEquipments.Add(eq12);
        OwnedEquipments.Add(eq13);
        OwnedEquipments.Add(eq14);
 

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
            character.CurrentState = ai.Data.CurrentState;
            character.CurrentStamina = ai.Data.CurrentStamina;
        }
    }

    #endregion

    #region Equipment

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
            AttachPreviewToCharacter(ai, previewEquipment);
        }
    }

    public void EquipItem(Character character, Equipment equipment)
    {
        if (character == null || equipment == null)
        {
            Debug.LogWarning("[EquipItem] character 또는 equipment 가 null입니다.");
            return;
        }

        var id = character.DataId;

        // 실제 데이터 참조
        var targetCharacter = CharacterMap.ContainsKey(id) ? CharacterMap[id] : Characters.Find(c => c.DataId == id);
        if (targetCharacter == null)
        {
            Debug.LogWarning($"[EquipItem] 캐릭터 {id} 을 찾을 수 없습니다.");
            return;
        }

        var type = equipment.EquipmentData.EquipmentType;

        // 기존에 이 장비를 착용하고 있던 캐릭터가 있다면 먼저 해제
        if (!string.IsNullOrEmpty(equipment.EquippedByCharacterId) &&
            equipment.EquippedByCharacterId != targetCharacter.DataId)
        {
            var previousOwner = Characters.Find(c => c.DataId == equipment.EquippedByCharacterId);
            if (previousOwner != null)
            {
                UnEquipItem(previousOwner, equipment);
            }
        }
        // 기존 장비가 있으면 먼저 해제
        if (targetCharacter.EquippedItems.TryGetValue(type, out var oldEquipment))
        {
            UnEquipItem(targetCharacter, oldEquipment);
        }

        // 데이터 반영
        targetCharacter.EquippedItems[type] = equipment;
        if (!targetCharacter.EquippedItemIds.Contains(equipment.key))
            targetCharacter.EquippedItemIds.Add(equipment.key);

        equipment.EquippedByCharacterId = targetCharacter.DataId; // 수정: 
        equipment.IsEquipped = true;

        // 시각화 반영
        if (CharactersInScene.TryGetValue(id, out var ai))
        {
            AttachEquipmentToCharacter(ai, equipment);
        }

        // 이벤트 및 저장
        OnCharacterChanged?.Invoke();
        EquipInfoChanged?.Invoke();
        SaveGame();
    }

    public void UnEquipItem(Character character, Equipment equipment)
    {
        var id = character.DataId;
        var targetCharacter = CharacterMap.ContainsKey(id) ? CharacterMap[id] : Characters.Find(c => c.DataId == id);
        if (targetCharacter == null) return;

        var type = equipment.EquipmentData.EquipmentType;

        // 1. 데이터 제거
        targetCharacter.EquippedItems.Remove(type);
        targetCharacter.EquippedItemIds.Remove(equipment.key);

        equipment.EquippedByCharacterId = null;
        equipment.IsEquipped = false;

        // 2. 시각화 제거
        if (CharactersInScene.TryGetValue(id, out var ai))
        {
            DetachEquipmentFromCharacter(ai, type);
        }

        // 3. 이벤트/저장
        OnCharacterChanged?.Invoke();
        EquipInfoChanged?.Invoke();
        SaveGame();
    }

    public void SetInitEquipment(AICharacter character)
    {
        // 복사본을 만듦
        var equippedIdsCopy = new List<string>(character.Data.EquippedItemIds);

        foreach (var equipId in equippedIdsCopy)
        {
            Equipment equip = OwnedEquipments.Find(e => e.key == equipId);
            if (equip == null)
            {
                Debug.LogWarning($"장착 장비 {equipId} 를 못 찾음");
                continue;
            }

            if (!character.Data.EquippedItems.ContainsKey(equip.EquipmentData.EquipmentType))
                character.Data.EquippedItems.Add(equip.EquipmentData.EquipmentType, equip);

            EquipItem(character.Data, equip);
        }
    }

    private void AttachPreviewToCharacter(AICharacter ai, Equipment equipment)
    {
        var type = equipment.EquipmentData.EquipmentType;

        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"[AttachPreviewToCharacter] 장비 본이 존재하지 않음: {type}");
            return;
        }

        // 기존 미리보기 장비 제거 (Equipped_로 시작하는 기존 시각화 삭제)
        foreach (Transform child in bone)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // 새 장비 시각화
        EquipmentController preview = Managers.Object.Spawn<EquipmentController>(
            Vector3.zero, equipment.EquipmentData.DataId, bone);
    }
    private void AttachEquipmentToCharacter(AICharacter ai, Equipment equipment)
    {
        var type = equipment.EquipmentData.EquipmentType;

        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"장비 본이 존재하지 않음: {type}");
            return;
        }

        foreach (Transform child in bone)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        EquipmentController go = Managers.Object.Spawn<EquipmentController>(Vector3.zero, equipment.EquipmentData.DataId, bone);

    }

    private void DetachEquipmentFromCharacter(AICharacter ai, EEquipmentType type)
    {
        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"장비 본이 없음 : {type}");
            return;
        }

        foreach (Transform child in bone)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
    }

    #endregion

    #region Gacha

    public string DrawRandomCreature()
    {
        var validList = new List<GachaData>();
        foreach (var pair in Managers.Data.GachaDic)
        {
            if (pair.Value.Probability > 0)
                validList.Add(pair.Value);
        }

        if (validList.Count == 0)
        {
            Debug.LogError("[Gacha] No available creatures!");
            return null;
        }

        float totalProb = 0f;
        foreach (var data in validList)
            totalProb += data.Probability;

        float rand = UnityEngine.Random.Range(0f, totalProb);
        float sum = 0f;

        foreach (var data in validList)
        {
            sum += data.Probability;
            if (rand <= sum)
            {
                Managers.Debug.Log($"[Gacha] 당첨! {data.DataId}",Define.EDebugType.UI);
                Managers.Debug.Log($"[Gacha] 확률: {data.Probability}, 총합: {totalProb}",Define.EDebugType.UI);
                return data.DataId;
            }
        }

        Debug.LogWarning("[Gacha] Fallback to first");
        return validList[0].DataId;
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

