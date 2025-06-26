using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardData
{
    public int Gold;
    public int Exp;
    public List<ItemReward> Items; // 필요하다면 아이템 보상도 포함
}

[Serializable]
public class ItemReward
{
    public string ItemId;
    public int Amount;
}