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
            cd.TotalExp = ConvertValue<int>(row[i++]);
            cd.MaxHp = ConvertValue<float>(row[i++]);
            cd.Atk = ConvertValue<float>(row[i++]);
            cd.MaxStamina = ConvertValue<float>(row[i++]);
            cd.MoveSpeed = ConvertValue<float>(row[i++]);
            cd.HpRate = ConvertValue<float>(row[i++]);
            cd.AtkRate = ConvertValue<float>(row[i++]);
            cd.MoveSpeedRate = ConvertValue<float>(row[i++]);
            cd.IconLabel = ConvertValue<string>(row[i++]);
            cd.SkillTypeList = ConvertList<string>(row[i++]);
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
