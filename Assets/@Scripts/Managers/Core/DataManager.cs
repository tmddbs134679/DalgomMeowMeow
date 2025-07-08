using Data;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<key, Value>
{
    Dictionary<key, Value> MakeDict();
}

public class DataManager 
{
    public Dictionary<string, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<string, Data.CreatureData>();
    public Dictionary<string, Data.FoodData> FoodDic { get; private set; } = new Dictionary<string, Data.FoodData>();

    public Dictionary<string, Data.EquipmentData> EquipmentDic { get; private set; } = new Dictionary<string, Data.EquipmentData>();
    public Dictionary<string, Data.BuildingData> BuildingDic { get; private set; } = new Dictionary<string, Data.BuildingData>();
    public Dictionary<string, Data.GachaData> GachaDic { get; private set; } = new();
    public Dictionary<(string, int), Data.BuildingLevelData> BuildingLevelDic { get; private set; } = new Dictionary<(string, int), Data.BuildingLevelData>();
    public Dictionary<int, CheckOutData> CheckOutDataDic { get; private set; } = new Dictionary<int, Data.CheckOutData>();
    public Dictionary<int, Data.MaterialData> MaterialDic { get; private set; } = new Dictionary<int, Data.MaterialData>();

    public void Init()
    {
        CreatureDic = LoadJson<Data.CreatureDataLoader, string, Data.CreatureData>("CreatureData").MakeDict();
        FoodDic = LoadJson<Data.FoodDataLoader, string, Data.FoodData>("FoodData").MakeDict();
        BuildingDic = LoadJson<Data.BuildingDataLoader, string, Data.BuildingData>("BuildingData").MakeDict();
        EquipmentDic = LoadJson<Data.EquipmentDataLoader, string, Data.EquipmentData>("EquipmentData").MakeDict();
        BuildingLevelDic = LoadJson<Data.BuildingLevelDataLoader, (string, int), Data.BuildingLevelData>("BuildingLevelData").MakeDict();
        GachaDic = LoadJson<Data.GachaDataLoader, string, Data.GachaData>("GachaData").MakeDict();
        CheckOutDataDic = LoadJson<Data.CheckOutDataLoader, int, Data.CheckOutData>("CheckOutData").MakeDict();
        MaterialDic = LoadJson<Data.MaterialDataLoader, int, Data.MaterialData>("MaterialData").MakeDict();
    }



    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }





}
