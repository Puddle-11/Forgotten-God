using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Decoration 
{
    public DecorationPreset.DecorationType D_Type;
    public float D_Density;
    public TileBase D_Tile;
    public Vector2Int D_GrowthDirection;
    public Vector2Int D_Height;

}
