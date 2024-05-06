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
    public Color foregroundColor, midgroundOneColor, midgroundTwoColor, backgroundOneColor, backgroundTwoColor, backdropColor;
    public GameObject[] godRay;
    public Vector2Int godRayCount;
    [Range(0, 100)]
    public float subRoomFrequency;
    [SerializeField] private Vector2Int numberOfDoors;
    [Space]
    [Space]
    [Header("Base Terrain Settings")]
    public float hillFrequency;
    public float hillMagnitude;
    public float hillHeight;
    public float edgeHillFalloff;


    [Space]
    [Space]
    [Header("Secondary Terrain Settings")]
    public float groundMinHeight;
    public float layerTwoDifference;


    [Space]
    [Space]
    [Header("Secondary Terrain Settings")]
    public float backgroundHeight;


    [Space]
    [Space]
    [Header("Decor Settings")]
    public float leafFrequency;
    [Range(0, 100)]
    public float leafDensity;
    public float leafCurve;
    public float leafEdgeFalloff;
    public float leafDepth;
    public float leafLayerTwoDifference;
    public float leafMagnitude;
    [Space]
    [Space]
    public float foreGroundMagnitude;
    public float foreGroundHeight;
    [Range(0, 100)]
    public float foreGroundLeafDensity;
    public float foreGroundEdgeFalloff;
    [Space]
    [Space]
    public float backgroundHillHeight;
    public float secondaryBackgroundHillDifference;
    public float backgroundLeafDepth;
    public float secondaryBackgroundLeafDepth;
    public float backgroundLeafFalloff;

    public Vector2Int GetCenter()
    {
        return size / 2;

    }
}
