using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class RangeInt

{

    public RangeInt(int _min, int _max)
    {
        minX = _min;
        maxX = _max;
    }
    public RangeInt(int _minX, int _maxX,int _minY, int _maxY)
    {
        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;
    }
    public RangeInt()
    {
        minX = 0;
        maxX = 0;
        minY = 0;
        maxY = 0;
    }

    public void SetX(int _min, int _max)
    {
        minX = _min;
        maxX = _max;
    }
    public void SetY(int _min, int _max)
    {
        minY = _min;
        maxY = _max;
    }
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;

}
