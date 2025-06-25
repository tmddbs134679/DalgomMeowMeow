using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;

    public List<AICharacter> AllCharacters  = new List<AICharacter>();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(AICharacter character)
    {
        if (character == null) return;
        if (!AllCharacters.Contains(character))
            AllCharacters.Add(character);
    }

    public void Unregister(AICharacter character)
    {
        if (character == null) return;
        AllCharacters.Remove(character);
    }


}
