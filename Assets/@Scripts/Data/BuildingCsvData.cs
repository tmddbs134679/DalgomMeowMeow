public enum BuildingType
{
    Cooking,
    Fishing,
    Resting,
    Farm,
    Shop
}

[System.Serializable]
public class BuildingCsvData
{
    public int Id;
    public string BuildingName;
    public BuildingType Type;
    public float Interval;
    public int ProduceItemId;
    public int UnlockCost;
}