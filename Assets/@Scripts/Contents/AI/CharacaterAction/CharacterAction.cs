using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    [SerializeField] private CharacterStatSo _character;
    private float _currentStamina;

    private void Awake()
    {
        _currentStamina = _character.Stamina;
    }


    public void Cook()
    {

    }

    public void Play()
    {

    }

    public void Rest()
    {

    }

    public void Deliver()
    {

    }

    public void Collect()
    {

    }

    public void Farm()
    {

    }

    public void Build()
    {

    }
}
