using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TileBounds
{
    public RangeFloat WorldBounds;
    public RangeInt CellBounds;
    public float scale = 2;
    public TileBounds(float FloatMin, float FloatMax, int IntMin, int IntMax)
    {
        WorldBounds = new RangeFloat(FloatMin, FloatMax);
        CellBounds = new RangeInt(IntMin, IntMax);
    }

    public TileBounds(float FloatMin, float FloatMax)
    {
        WorldBounds = new RangeFloat(FloatMin, FloatMax);
        CellBounds = new RangeInt();
    }
    public TileBounds(int IntMin, int IntMax)
    {
        WorldBounds = new RangeFloat();
        CellBounds = new RangeInt(IntMin, IntMax);
    }
    public TileBounds()
    {
        WorldBounds = new RangeFloat();
        CellBounds = new RangeInt();
    }

}
