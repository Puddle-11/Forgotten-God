using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Custom Objects/Room Preset")]
public class RoomPreset : ScriptableObject
{
    [Header("General Settings")]
    public Vector2Int size;
    public TileBase baseTile;
    public TileBase leafTile;
    public Sprite colorTexture;
    public GameObject[] godRay;
    public Vector2Int godRayCount;
    public int maxExits;

    public BiomeLayer[] layerValues;







  
    public Vector2Int GetCenter()
    {
        return size / 2;

    }
}
[System.Serializable]
public class BiomeLayer
{
    public string L_Name = "Untitled Layer";
    public float L_terrainScale = 24;
    public float L_magnitude = 6.5f;
    public float L_height = 3;
    public float L_falloff = -0.1f;
    public float L_scale = 2;
    public float L_leafScale = 24;
    public float L_leafMagnitude = 6.5f;
    public float L_leafDepth = 5;
    public float L_leafFalloff = 0.1f;
}