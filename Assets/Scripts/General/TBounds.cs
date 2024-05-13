using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[System.Serializable]
public class TBounds
{
    public float Scale = 2;
    public floatRange WorldX;
    public floatRange WorldY;
    public intRange CellX;
    public intRange CellY;

    public void SetWorld(floatRange _worldx, floatRange _worldy)
    {
        WorldX = _worldx;
        WorldY = _worldy;
    }
    public void SetCell(intRange _cellx, intRange _celly)
    {
        CellX = _cellx;
        CellY = _celly;
    }
    public TBounds(float _scale)
    {

        Scale = _scale;
    }

    public TBounds(float _scale, float _sizeX, float _sizeY)
    {
        Scale = _scale;
        WorldX = new floatRange(-_sizeX, _sizeX);
        WorldY = new floatRange(-_sizeY, _sizeY);


        CellX = new intRange(-_sizeX / Scale, _sizeX / Scale);
        CellY = new intRange(-_sizeY / Scale, _sizeY / Scale);

    }
}
[System.Serializable]

public class floatRange
{
    public float min;
    public float max;

    public floatRange(float _min, float _max)
    {
        min = _min;
        max = _max;
    }

}
[System.Serializable]

public class intRange
{
    public int min;
    public int max;
    public intRange(int _min, int _max)
    {
        min = _min;
        max = _max;
    }
    public intRange(float _min, float _max)
    {
        min = (int)_min;
        max = (int)_max;
    }

}
