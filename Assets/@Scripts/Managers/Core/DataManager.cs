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

    public Dictionary<string, Data.BuildingData> BuildingDic { get; private set; } = new Dictionary<string, Data.BuildingData>();
    public void Init()
    {
        CreatureDic = LoadJson<Data.CreatureDataLoader, string, Data.CreatureData>("CreatureData").MakeDict();
        FoodDic = LoadJson<Data.FoodDataLoader, string, Data.FoodData>("FoodData").MakeDict();
        BuildingDic = LoadJson<Data.BuildingDataLoader, string, Data.BuildingData>("BuildingData").MakeDict();
    }



    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }





}
