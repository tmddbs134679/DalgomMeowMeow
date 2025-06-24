using Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR

    [MenuItem("Tools/ParseExcel ")]
    public static void ParseExcel()
    {
        ParseCreatureData("Creature");
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
            cd.DataId = ConvertValue<int>(row[i++]);
            cd.PrefabLabel = ConvertValue<string>(row[i++]);
            cd.MaxHp = ConvertValue<float>(row[i++]);
            cd.Hp = ConvertValue<float>(row[i++]);
            cd.Atk = ConvertValue<float>(row[i++]);
            cd.Stamina = ConvertValue<float>(row[i++]);
            cd.MoveSpeed = ConvertValue<float>(row[i++]);
            cd.TotalExp = ConvertValue<float>(row[i++]);
            cd.HpRate = ConvertValue<float>(row[i++]);
            cd.AtkRate = ConvertValue<float>(row[i++]);
            cd.MoveSpeedRate = ConvertValue<float>(row[i++]);
            cd.IconLabel = ConvertValue<string>(row[i++]);
            loader.creatures.Add(cd);
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

#endif

}
