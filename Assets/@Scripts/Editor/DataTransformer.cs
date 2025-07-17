using Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Define;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR

    [MenuItem("Tools/ParseExcel ")]
    public static void ParseExcel()
    {
        ParseCreatureData("Creature");
        ParseFoodData("Food");
        ParseBuildingData("Building");
        ParseEquipmentData("Equipment");
        ParseGachaData("Gacha");
        ParseGachaStatData("GachaStat");
        ParseCheckOutData("CheckOut");
        ParseMaterialData("Material");
        ParseEquipmentGacha("EquipmentGacha");
        ParseSkillData("Skill");
        ParseBuidingLevelData("BuildingLevel");
        ParseQuestData("Quest");
        ParseUnlockContentsData("UnlockContents");
        
    }

    private static void ParseMaterialData(string filename)
    {
        MaterialDataLoader loader = new MaterialDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            MaterialData material = new MaterialData();
            material.DataId = ConvertValue<int>(row[i++]);
            material.MaterialType = ConvertValue<Define.EMaterialType>(row[i++]);
            material.NameTextID = ConvertValue<string>(row[i++]);
            material.DescriptionTextID = ConvertValue<string>(row[i++]);
            material.SpriteName = ConvertValue<string>(row[i++]);

            loader.Materials.Add(material);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static void ParseCheckOutData(string filename)
    {
        CheckOutDataLoader loader = new CheckOutDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            CheckOutData chk = new CheckOutData();
            chk.Day = ConvertValue<int>(row[i++]);
            chk.RewardItemId = ConvertValue<int>(row[i++]);
            chk.RewardItemValue = ConvertValue<int>(row[i++]);

            loader.checkouts.Add(chk);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseFoodData(string filename)
    {
        FoodDataLoader loader = new FoodDataLoader();

        #region FoodData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            Data.FoodData data = new Data.FoodData();
            data.DataId = ConvertValue<string>(row[i++]);
            data.Name = ConvertValue<string>(row[i++]);
            data.Description = ConvertValue<string>(row[i++]);
            data.SpriteName = ConvertValue<string>(row[i++]);
            data.Price = ConvertValue<int>(row[i++]);
            data.Count = ConvertValue<int>(row[i++]);   
            loader.foods.Add(data);
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
        #endregion
    }

    static void ParseCreatureData(string filename)
    {
        CreatureDataLoader loader = new CreatureDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            CreatureData cd = new CreatureData();
            cd.DataId = ConvertValue<string>(row[i++]);
            cd.PrefabLabel = ConvertValue<string>(row[i++]);
            cd.Name = ConvertValue<string>(row[i++]);
            cd.MaxExp = ConvertValue<int>(row[i++]);
            cd.MaxHp = ConvertValue<float>(row[i++]);
            cd.Atk = ConvertValue<float>(row[i++]);
            cd.MaxStamina = ConvertValue<float>(row[i++]);
            cd.MoveSpeed = ConvertValue<float>(row[i++]);
            cd.HpRate = ConvertValue<float>(row[i++]);
            cd.AtkRate = ConvertValue<float>(row[i++]);
            cd.MoveSpeedRate = ConvertValue<float>(row[i++]);
            cd.IconLabel = ConvertValue<string>(row[i++]);
            cd.SkillID = ConvertValue<string>(row[i++]);
            cd.WalkSpeed = ConvertValue<float>(row[i++]);
            loader.creatures.Add(cd);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }


    private static void ParseBuildingData(string filename)
    {
        BuildingDataLoader loader = new BuildingDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            BuildingData building = new BuildingData();
            building.DataId = ConvertValue<string>(row[i++]);
            building.Name = ConvertValue<string>(row[i++]); 
            building.Type = ConvertValue<Define.EBuildingType>(row[i++]);
            building.Description = ConvertValue<string>(row[i++]);
            building.BuildTime = ConvertValue<float>(row[i++]);
            building.ProductionTime = ConvertValue<float>(row[i++]);
            String[] sizeParts = row[i++].Split('&');
            building.Size = new Vector2Int(int.Parse(sizeParts[0]), int.Parse(sizeParts[1]));
            building.DataName = ConvertValue<string>(row[i++]);
            building.UnlockLevel = ConvertValue<int>(row[i++]);
            building.MaxLevel = ConvertValue<int>(row[i++]);
            building.Exp = ConvertValue<int>(row[i++]); 
            loader.buildings.Add(building);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseEquipmentData(string filename)
    {
        EquipmentDataLoader loader = new EquipmentDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            EquipmentData eq = new EquipmentData();
            eq.DataId = ConvertValue<string>(row[i++]);
            eq.EquipmentType = ConvertValue<Define.EEquipmentType>(row[i++]);
            eq.Name = ConvertValue<string>(row[i++]);
            eq.Description = ConvertValue<string>(row[i++]);
            eq.SpriteName = ConvertValue<string>(row[i++]);
            loader.Equipments.Add(eq);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseGachaData(string filename)
    {
        GachaDataLoader loader = new GachaDataLoader();
        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;
            int i = 0;

            GachaData gc = new GachaData();
            gc.DataId = ConvertValue<string>(row[i++]);
            gc.Probability = ConvertValue<float>(row[i++]);
            loader.Gachas.Add(gc);


        }
        #endregion
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseGachaStatData(string filename)
    {
        GachaStatDataLoader loader = new GachaStatDataLoader();
        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;
            int i = 0;

            GachaStatData gc = new GachaStatData();
            gc.DataId = ConvertValue<string>(row[i++]);
            gc.HpMin = ConvertValue<float>(row[i++]);
            gc.HpMax = ConvertValue<float>(row[i++]);
            gc.AtkMin = ConvertValue<float>(row[i++]);
            gc.AtkMax = ConvertValue<float>(row[i++]);
            gc.MoveSpeedMin = ConvertValue<float>(row[i++]);
            gc.MoveSpeedMax = ConvertValue<float>(row[i++]);
            gc.StaminaMin = ConvertValue<float>(row[i++]);
            gc.StaminaMax = ConvertValue<float>(row[i++]);

            loader.GachaStats.Add(gc);


        }
        #endregion
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }




    private static void ParseEquipmentGacha(string filename)
    {
        EquipmentGachaDataLoader loader = new EquipmentGachaDataLoader();

        #region EquipmentGachaData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            EquipmentGachaData Gacha = new EquipmentGachaData();
            Gacha.EquipmentID = ConvertValue<string>(row[i++]);
            Gacha.GachaRate = ConvertValue<float>(row[i++]);
            Gacha.Grade = ConvertValue<Define.EEquipmentGrade>(row[i++]);   

            loader.EquipmentGachaTable.Add(Gacha);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static void ParseSkillData(string filename)
    {
        SkillDataDataLoader loader = new SkillDataDataLoader();
    
        #region ParseSkillData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    
        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;
    
            int i = 0;
    
            SkillData skill = new SkillData();
            skill.DataId = ConvertValue<string>(row[i++]);
            skill.Name = ConvertValue<string>(row[i++]);
            skill.Description = ConvertValue<string>(row[i++]);
            skill.CoolTime = ConvertValue<float>(row[i++]);
    
            loader.skills.Add(skill);
        }
        #endregion
    
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static void ParseBuidingLevelData(string filename)
    {
        BuildingLevelDataLoader loader = new BuildingLevelDataLoader();
    
        #region ParseBuildingLevelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    
        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;
    
            int i = 0;
    
            BuildingLevelData buildingLevel = new BuildingLevelData();
            buildingLevel.BuildingId = ConvertValue<string>(row[i++]);
            buildingLevel.Level = ConvertValue<int>(row[i++]);
            buildingLevel.UpgradeCost = ConvertValue<int>(row[i++]);
            buildingLevel.ProducedFoodId = ConvertValue<string>(row[i++]);
    
            loader.levels.Add(buildingLevel);
        }
        #endregion
    
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    
    private static void ParseQuestData(string filename)
    {
        QuestDataLoader loader = new QuestDataLoader();
    
        #region ParseQuestData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    
        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;
    
            int i = 0;
    
            QuestData quest = new QuestData();
            quest.QuestId = ConvertValue<string>(row[i++]);
            quest.Title = ConvertValue<string>(row[i++]);
            quest.QuestType = ConvertValue<EQuestType>(row[i++]);
            quest.QuestConditionType = ConvertValue<EQuestConditionType>(row[i++]);
            quest.TargetType = ConvertValue<ETargetType>(row[i++]);
            quest.GoalCount = ConvertValue<int>(row[i++]);
            quest.Reward = ConvertValue<int>(row[i++]);
            quest.PreviousQuestID = ConvertValue<string>(row[i++]);
    
            loader.quests.Add(quest);
        }
        #endregion
    
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    
    private static void ParseUnlockContentsData(string filename)
    {
        UnlockContentsDataLoader loader = new UnlockContentsDataLoader();
        var contentDict = new Dictionary<string, UnlockContentsData>();
    
        #region ParseUnlockContentsData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    
        for (int y = 1; y < lines.Length; y++) // Skip header
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length < 4 || string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            string contentId = ConvertValue<string>(row[i++]);
            UnlockConditionType type = ConvertValue<UnlockConditionType>(row[i++]);
            string questId = ConvertValue<string>(row[i++]);
            int requiredGold = ConvertValue<int>(row[i++]);

            var condition = new UnlockCondition
            {
                Type = type,
                QuestId = questId,
                RequiredGold = requiredGold
            };

            if (!contentDict.TryGetValue(contentId, out var unlockData))
            {
                unlockData = new UnlockContentsData
                {
                    ContentId = contentId,
                    Conditions = new List<UnlockCondition>()
                };
                contentDict[contentId] = unlockData;
            }

            unlockData.Conditions.Add(condition);
        }

        loader.UnlockContents = new List<UnlockContentsData>(contentDict.Values);
        #endregion
    
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    public static T ConvertValue<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }
    public static List<T> ConvertList<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new List<T>();

        return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
    }



#endif





}
