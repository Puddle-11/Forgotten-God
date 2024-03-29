using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
[System.Serializable]

public class RangeFloat
{
    public RangeFloat(float _min, float _max)
    {
        minX = _min;
        maxX = _max;
    }
    public RangeFloat(float _minX, float _maxX,float _minY, float _maxY)
    {
        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;
    }

    public RangeFloat()
    {
        minX = 0;
        maxX = 0;
    }
    public void SetX(float _min, float _max)
    {
        minX = _min;
        maxX = _max;
    }
    public void SetY(float _min, float _max)
    {
        minY = _min;
        maxY = _max;
    }
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
}
