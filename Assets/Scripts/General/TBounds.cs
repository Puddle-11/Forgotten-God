using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBounds
{
    private float Scale;
    
    TBounds(float _scale)
    {
        Scale = _scale;
    }
}



public class floatRange
{
    private float min { get; }
    private float max { get; }

    public floatRange(float _min, float _max)
    {
        min = _min;
        max = _max;
    }

}
public class intRange
{
    private int min { get; }
    private int max { get; }
    public intRange(int _min, int _max)
    {
        min = _min;
        max = _max;
    }
}
