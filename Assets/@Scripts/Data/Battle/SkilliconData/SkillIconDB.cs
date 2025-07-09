using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/IconDatabase")]
public class SkillIconDB : ScriptableObject
{
    public List<SkillIconData> iconList;
}

[System.Serializable]
public class SkillIconData
{
    public string id;
    public Sprite icon;
}