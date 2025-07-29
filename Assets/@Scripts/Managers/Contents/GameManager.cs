using Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;
using static Define;
using Random = System.Random;

[Serializable]
public class GameData
{
    public float Gold = 0;
    public int Dia = 0;
    public int Ticket = 0;

    //public List <캐릭터들>

    // 저장 전용
    public List<Character> CharacterList = new List<Character>();
    public List<Equipment> OwnedEquipments = new List<Equipment>();

    public bool BGMOn = true;
    public bool EffectSoundOn = true;

    public int MaxCountInScene = 3;

    public bool[] AttendanceReceived = new bool[30];
    public int AdvancedGachaOpenCount = 0;
    public int offineRewardGold = 0;

}


public class GameManager
{
    public GameData _gameData = new GameData();


    public Dictionary<string, Character> _characters = new Dictionary<string, Character>();

    public Dictionary<string, Character> CharacterMap = new();

    public Dictionary<string, AICharacter> AllCharacter = new();
    // 씬에 존재하는 실제 캐릭터 오브젝트
    public Dictionary<string, AICharacter> CharacterInMainScene = new();

    public Dictionary<string, AICharacter> CharacterInStoreScene = new();

    public bool IsLoaded = false;

    // 씬에 존재하는 최대 캐릭터 수, 5명으로 제한
    public int MainSceneCount = 0; // 메인 씬에 존재하는 캐릭터 수

    #region Action

    public event Action OnResourcesChagned;
    public Action OnCharacterChanged;

    #endregion

    #region GameData

    public float Gold
    {
        get { return _gameData.Gold; }
        set
        {
            _gameData.Gold = value;
            QuestManager.Instance.TryUnlockByGold(); // 해금 골드조건 체크
            SaveGame();
            OnResourcesChagned?.Invoke();
        }
    }

    public int Dia
    {
        get { return _gameData.Dia; }
        set
        {
            _gameData.Dia = value;
            SaveGame();
            OnResourcesChagned?.Invoke();
        }
    }

    public int Ticket
    {
        get { return _gameData.Ticket; }
        set
        {
            _gameData.Ticket = value;
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

            Managers.Equipment.EquipInfoChanged?.Invoke();
        }
    }

    public bool[] AttendanceReceived
    {
        get { return _gameData.AttendanceReceived; }
        set { _gameData.AttendanceReceived = value; }

    }

    public int AdvancedGachaOpenCount
    {
        get { return _gameData.AdvancedGachaOpenCount; }
        set
        {
            _gameData.AdvancedGachaOpenCount = value;
        }
    }

    public int IncreaseMaxCountInScene
    {
        get { return _gameData.MaxCountInScene; }
        set
        {
            _gameData.MaxCountInScene = value;
            Managers.UI.ShowToast($"최대 캐릭터 수가 {_gameData.MaxCountInScene}명으로 증가했습니다.");
            SaveGame();
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

                _characters[character.UniqueId] = character;
            }

            foreach (var equip in _gameData.OwnedEquipments)
            {
                if (string.IsNullOrEmpty(equip.UniqueId))
                    equip.UniqueId = $"EQ_{Guid.NewGuid().ToString().Substring(0, 8)}";

                equip.SetInfo(Managers.Data.EquipmentDic[equip.key]);
            }


            return;
        }

        #region 초기 생성 Test
        // 최초 생성
        var newChar = new Character();
        newChar.InMainScene = true; // 메인 씬에 존재하는 캐릭터로 설정
        newChar.Init("A10001", new Vector3(39f, 0, 27f)); // 위치 초기값
        newChar.SetInfo(Managers.Data.CreatureDic["A10001"]);
        newChar.IsConfirmed = true;
        _characters[newChar.UniqueId] = newChar;

        var newCharTest = new Character();
        newCharTest.InMainScene = true; // 메인 씬에 존재하는 캐릭터로 설정
        newCharTest.Init("A10002", new Vector3(39f, 0, 27f)); // 위치 초기값
        newCharTest.SetInfo(Managers.Data.CreatureDic["A10002"]);
        newCharTest.IsConfirmed = true;
        _characters[newCharTest.UniqueId] = newCharTest;

        var newChar1 = new Character();
        newChar1.InMainScene = true; // 메인 씬에 존재하는 캐릭터로 설정
        newChar1.Init("A10006", new Vector3(38f, 0, 27f)); // 위치 초기값
        newChar1.SetInfo(Managers.Data.CreatureDic["A10006"]);
        newChar1.IsConfirmed = true;
        _characters[newChar1.UniqueId] = newChar1;


        AdvancedGachaOpenCount = 3;

        //var newChar2 = new Character();
        //newChar2.Init("A10003", new Vector3(38f, 0, 27)); // 위치 초기값
        //newChar2.SetInfo(Managers.Data.CreatureDic["A10003"]);
        //_characters[newChar2.UniqueId] = newChar2;

        //var newChar3 = new Character();
        //newChar3.Init("A10001", new Vector3(38f, 0, 27)); // 위치 초기값
        //newChar3.SetInfo(Managers.Data.CreatureDic["A10001"]);
        //_characters[newChar3.UniqueId] = newChar3;

        //// 초기 장비 생성
        //Equipment eq1 = new Equipment("E0001");
        //Equipment eq2 = new Equipment("E0002");
        //Equipment eq3 = new Equipment("E0003");
        //Equipment eq4 = new Equipment("E0004");
        //Equipment eq5 = new Equipment("E0005");
        //Equipment eq6 = new Equipment("E0006");
        //Equipment eq7 = new Equipment("E0007");
        //Equipment eq8 = new Equipment("E0101");
        //Equipment eq9 = new Equipment("E0102");
        //Equipment eq10 = new Equipment("E0201");
        //Equipment eq11 = new Equipment("E0202");
        //Equipment eq12 = new Equipment("E0203");
        //Equipment eq13 = new Equipment("E0205");
        //Equipment eq14 = new Equipment("E0207");
        //Equipment eq15 = new Equipment("E0001");
        //Equipment eq16 = new Equipment("E0001");

        //OwnedEquipments.Add(eq1);
        //OwnedEquipments.Add(eq2);
        //OwnedEquipments.Add(eq3);
        //OwnedEquipments.Add(eq4);
        //OwnedEquipments.Add(eq5);
        //OwnedEquipments.Add(eq6);
        //OwnedEquipments.Add(eq7);
        //OwnedEquipments.Add(eq8);
        //OwnedEquipments.Add(eq9);
        //OwnedEquipments.Add(eq10);
        //OwnedEquipments.Add(eq11);
        //OwnedEquipments.Add(eq12);
        //OwnedEquipments.Add(eq13);
        //OwnedEquipments.Add(eq14);
        //OwnedEquipments.Add(eq15);
        //OwnedEquipments.Add(eq16);

        #endregion

        Gold += 100050;

        SaveGame();
        IsLoaded = true;
    }



    public void SaveGame()
    {

        Managers.Time.LastQuitTime = DateTime.Now;

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
            CharacterMap = _gameData.CharacterList.ToDictionary(c => c.UniqueId, c => c);
        }


        IsLoaded = true;
        return true;
    }

    public void UpdateCharactersFromWorld()
    {
        foreach (var pair in _characters)
        {
            Character character = pair.Value;

            if (!Managers.Game.CharacterInMainScene.TryGetValue(character.UniqueId, out AICharacter ai))
                continue;

            if (ai == null)
                return;

            if (Managers.Scene.CurrentScene is GameScene)
            {
                character.Pos = new Vector3Data(ai.transform.position);
                character.CurrentState = ai.Data.CurrentState;
                character.CurrentStamina = ai.Data.CurrentStamina;
            }

            if (Managers.Scene.CurrentScene is CharacterStoreScene)
            {
                character.RoomPos = new Vector3Data(ai.transform.position);
            }
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
            return null;
        }

        float totalProb = 0f;
        foreach (var data in validList)
            totalProb += data.Probability;

        float rand = UnityEngine.Random.value;
        float sum = 0;
        foreach (var data in validList)
        {
            sum += data.Probability;
            if (rand <= sum)
            {
                Managers.Debug.Log($"[Gacha] 당첨! {data.DataId}", Define.EDebugType.UI);
                Managers.Debug.Log($"[Gacha] 확률: {data.Probability}, 총합: {totalProb}", Define.EDebugType.UI);
                return data.DataId;
            }
        }
        return null;
    }


    public Character SpawnRandomGachaCharacter()
    {
        string creatureId = DrawRandomCreature();

        QuestManager.Instance.UpdateQuestProgress(Define.EQuestConditionType.Collect, Define.ETargetType.Animal);

        if (!Managers.Data.CreatureDic.TryGetValue(creatureId, out var creatureData))
        {
            return null;
        }

        Character newChar = new Character();
        newChar.Init(creatureId, new Vector3(39f, 1, 27f)); // 위치 초기값
        newChar.SetInfo(creatureData);

        ApplyRandomStat(newChar);

        int currentInScene = Characters.Count(c => c.InMainScene);

        if (currentInScene >= IncreaseMaxCountInScene)
        {
            newChar.InMainScene = false;
            _characters[newChar.UniqueId] = newChar;
            Managers.Debug.Log($"[Gacha] 고양이 보관함에 저장됨: {newChar.DataId}", Define.EDebugType.AI);
            SaveGame();
            return newChar;
        }

        newChar.InMainScene = true;
        AICharacter aiChar = Managers.Object.Spawn<AICharacter>(new Vector3(39f, 1, 27f), creatureId, isReplica: false);

        if (aiChar == null)
        {
            return null;
        }
        aiChar.Init();
        aiChar.SetInfo(newChar);


        Managers.AI.Register(aiChar);
        _characters[newChar.UniqueId] = newChar;

        return newChar;
    }

    public void ApplyRandomStat(Character newChar)
    {
        var statRange = Managers.Data.GachaStatDataDic["S10001"];

        float deltaHp = UnityEngine.Random.Range(statRange.HpMin, statRange.HpMax);
        int signHp = UnityEngine.Random.value < 0.5f ? -1 : 1;
        newChar.Hp += (signHp * deltaHp);
        newChar.Hp = Mathf.Floor(newChar.Hp);

        float deltaStamina = UnityEngine.Random.Range(statRange.StaminaMin, statRange.StaminaMax);
        int signStamina = UnityEngine.Random.value < 0.5f ? -1 : 1;
        newChar.MaxStamina += signStamina * deltaStamina;
        newChar.MaxStamina = Mathf.Floor(newChar.MaxStamina);


        newChar.Atk = Mathf.Floor(UnityEngine.Random.Range(statRange.AtkMin, statRange.AtkMax));

        newChar.MoveSpeed = UnityEngine.Random.Range(statRange.MoveSpeedMin, statRange.MoveSpeedMax);
        newChar.MoveSpeed = (float)Math.Round(newChar.MoveSpeed, 1);
    }

    public List<Equipment> DoEquipmentGacha(int count)
    {
        List<Equipment> equipments = new List<Equipment>();


        EEquipmentGrade grade = GetRandomGrade(COMMON_GACHA_GRADE);

        var gachaEntries = Managers.Data.GachaTableDataDic.Values.
            Where(item => item.Grade == grade).ToList();

        UI_CheckOutRewardPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutRewardPopup>();

        for (int i = 1; i <= count; i++)
        {
            int index = UnityEngine.Random.Range(0, gachaEntries.Count);
            string key = gachaEntries[index].EquipmentID;

            if (Managers.Data.EquipmentDic.ContainsKey(key))
            {
                equipments.Add(Managers.Equipment.AddEquipment(key));
            }

            popup.SetInfo(Managers.Equipment.AddEquipment(key));
        }


        return equipments;
    }

    public List<Character> DoCharacterGacha(int count)
    {

        UI_CheckOutRewardPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutRewardPopup>();
        for (int i = 1; i <= count; i++)
        {
            Character character = SpawnRandomGachaCharacter();
            popup.SetInfo(character);
        }

        OnCharacterChanged?.Invoke();
        SaveGame();


        return null;

    }

    public EEquipmentGrade GetRandomGrade(float[] prob)
    {
        float randomValue = UnityEngine.Random.value;
        if (randomValue < prob[(int)EEquipmentGrade.Common])
        {
            return EEquipmentGrade.Common;
        }
        else if (randomValue < prob[(int)EEquipmentGrade.Common] + prob[(int)EEquipmentGrade.Uncommon])
        {
            return EEquipmentGrade.Uncommon;
        }
        else if (randomValue < prob[(int)EEquipmentGrade.Common] + prob[(int)EEquipmentGrade.Uncommon] + prob[(int)EEquipmentGrade.Rare])
        {
            return EEquipmentGrade.Rare;
        }
        else if (randomValue < prob[(int)EEquipmentGrade.Common] + prob[(int)EEquipmentGrade.Uncommon] + prob[(int)EEquipmentGrade.Rare] + prob[(int)EEquipmentGrade.Epic])
        {
            return EEquipmentGrade.Epic;
        }

        return EEquipmentGrade.Common;
    }


    public void RemoveTicket(int count)
    {
        Ticket -= count;
        SaveGame();
    }


    #endregion

    #region Option
    public bool BGMOn
    {
        get { return _gameData.BGMOn; }
        set
        {
            if (_gameData.BGMOn == value)
                return;
            _gameData.BGMOn = value;
            if (_gameData.BGMOn == false)
            {
                Managers.Sound.Stop(ESound.Bgm);
            }
            else
            {
                string name = "BGM1";
                if (Managers.Scene.CurrentScene.SceneType == Define.EScene.GameScene)
                    name = "BGM1";

                Managers.Sound.Play(Define.ESound.Bgm, name);
            }
        }
    }

    public bool EffectSoundOn
    {
        get { return _gameData.EffectSoundOn; }
        set { _gameData.EffectSoundOn = value; }
    }


    #endregion

    #region Reward

    public void RewardMaterial(int rewardId, int count)
    {

        EMaterialType type = Managers.Data.MaterialDic[rewardId].MaterialType;
        switch (type)
        {
            case EMaterialType.Gold:
                Gold += count;
                break;
            case EMaterialType.Dia:
                Dia += count;
                break;
            case EMaterialType.Ticket:
                Ticket += count;
                break;
        }
    }


    #endregion

    #region Travel

    public void StartTravel(Character character, TimeSpan duration)
    {
        character.IsTravelMode = true;

        Managers.Time.TravelStartTime = DateTime.Now;
        Managers.Time.TravelDuration = duration;

        if (Managers.Game.CharacterInMainScene.TryGetValue(character.UniqueId, out AICharacter aiCharacter))
        {
            //일단 false;
            aiCharacter.gameObject.SetActive(false);
        }

        Managers.Game.SaveGame();
    }



    public void ReturnFromTravel()
    {
        Character travelingCharacter = Managers.Game.Characters.FirstOrDefault(c => c.IsTravelMode);

        if (travelingCharacter == null)
        {
            Debug.Log("여행 중인 캐릭터가 없습니다.");
            return;
        }

        travelingCharacter.IsTravelMode = false;

        //복귀
        if (Managers.Game.CharacterInMainScene.TryGetValue(travelingCharacter.UniqueId, out AICharacter aiCharacter))
        {
            aiCharacter.gameObject.SetActive(true);
        }

        Managers.Game.SaveGame();
    }

    public void OnTravelComplete()
    {
        ReturnFromTravel();

        //보상 테이블에서  랜덤으로 주기 일단 테이블 없이 진행함.
        List<EMaterialType> allRewards = new List<EMaterialType>
        {
            EMaterialType.Gold,
            EMaterialType.Dia,
            EMaterialType.Ticket
        };

        List<EMaterialType> selectedRewards = allRewards
        .OrderBy(x => UnityEngine.Random.value)
        .Take(2)
        .ToList();

        StringBuilder rewardMessage = new StringBuilder("여행 보상: ");

        foreach (EMaterialType item in selectedRewards)
        {
            switch (item)
            {
                case EMaterialType.Gold: //테이블에 GOLD ID = 10000임.
                    int GoldValue = UnityEngine.Random.Range(2000, 5001);
                    Managers.Game.RewardMaterial(10000, GoldValue);
                    rewardMessage.Append($"Gold {GoldValue}, ");
                    break;
                case EMaterialType.Dia:
                    int DiaValue = UnityEngine.Random.Range(200, 1000);
                    Managers.Game.RewardMaterial(10001, DiaValue);
                    rewardMessage.Append($"Dia {DiaValue}, ");
                    break;
                case EMaterialType.Ticket:
                    int TicketValue = UnityEngine.Random.Range(1, 3);
                    Managers.Game.RewardMaterial(10002, TicketValue);
                    rewardMessage.Append($"Ticket {TicketValue}, ");
                    break;
            }
        }

        //마지막 쉼표 지우기
        if (rewardMessage.Length >= 2)
            rewardMessage.Length -= 2;

        Managers.UI.ShowToast(rewardMessage.ToString());
    }
    #endregion

}

