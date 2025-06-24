using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTimer
{
    private float _current;
    private float _max;

    public BuildingTimer(float max)
    {
        this._max = max;
        _current = 0f;
    }

    public bool Tick(float deltaTime)
    {
        _current += deltaTime;
        if (_current >= _max)
        {
            _current = 0f;
            return true;
        }
        return false;
    }
    


    public float GetProgressRatio() => _current / _max;
}
