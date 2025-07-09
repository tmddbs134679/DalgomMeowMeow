using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterEmoSet", menuName = "ScriptableObjects/CharacterEmoSet", order = 0)]
public class CharacterEmoSet : ScriptableObject
{
    public Material[] EmotionMaterials;
}
