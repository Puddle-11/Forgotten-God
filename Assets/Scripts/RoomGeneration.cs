using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using JetBrains.Annotations;
using UnityEditor.Presets;
using UnityEngine.UIElements;
using System.Xml;
using Unity.Mathematics;
using UnityEngine.Experimental.AI;

public class RoomGeneration : MonoBehaviour
{

    [SerializeField] private RoomPreset currentPreset;
    [SerializeField] private DecorationPreset currentDecorPreset;
    [Space]
    [Space]

    [SerializeField] private Tilemap decorTilemapOne;
    [SerializeField] private Tilemap groundTilemapOne;

    [Space]
    [Space]
    [SerializeField] private Tilemap decorTilemapTwo;
    [SerializeField] private Tilemap groundTilemapTwo;

    [Space]
    [Space]
    [SerializeField] private Tilemap foregroundTilemap;
    [Space]
    [Space]
    [SerializeField] private Tilemap backgroundOneTilemap;
    [Space]
    [Space]
    [SerializeField] private Tilemap backgroundTwoTilemap;
    public bool generate;


    private int hillSeed;
    private System.Random rand;
    private TileBounds mainMapBounds = new TileBounds();
    private TileBounds foregroundMapBounds = new TileBounds();
    private TileBounds backgroundMapBounds = new TileBounds();
    private List<GameObject> allGodRays = new List<GameObject>();

    private Vector2Int[] CornerMap = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
    };

    private void Start()
    {
        rand = new System.Random();
        //========================================================================================
        //Set Main Bounds
        mainMapBounds.CellBounds.SetX(-currentPreset.size.x, currentPreset.size.x);
        mainMapBounds.CellBounds.SetY(-currentPreset.size.y, currentPreset.size.y);
        mainMapBounds.WorldBounds.SetY(groundTilemapOne.CellToWorld(new Vector3Int(0, mainMapBounds.CellBounds.minY, 0)).y, groundTilemapOne.CellToWorld(new Vector3Int(0, mainMapBounds.CellBounds.maxY, 0)).y);
        mainMapBounds.WorldBounds.SetX(groundTilemapOne.CellToWorld(new Vector3Int(mainMapBounds.CellBounds.minX, 0, 0)).x, groundTilemapOne.CellToWorld(new Vector3Int(mainMapBounds.CellBounds.maxX, 0, 0)).x);
        //========================================================================================


        //========================================================================================
        //Set Foreground Bounds
        foregroundMapBounds.CellBounds.SetX(foregroundTilemap.WorldToCell(new Vector3(mainMapBounds.WorldBounds.minX,  0, 0)).x, foregroundTilemap.WorldToCell(new Vector3(mainMapBounds.WorldBounds.maxX, 0, 0)).x);
        foregroundMapBounds.CellBounds.SetY(foregroundTilemap.WorldToCell(new Vector3(0, mainMapBounds.WorldBounds.minY, 0)).y, foregroundTilemap.WorldToCell(new Vector3(0, mainMapBounds.WorldBounds.maxY, 0)).y);
        foregroundMapBounds.WorldBounds.SetY(mainMapBounds.WorldBounds.minY, mainMapBounds.WorldBounds.maxY);
        foregroundMapBounds.WorldBounds.SetX(mainMapBounds.WorldBounds.minX, mainMapBounds.WorldBounds.maxX);
        //========================================================================================
        //========================================================================================
        //Set Background Bounds

        backgroundMapBounds.CellBounds.SetY(backgroundOneTilemap.WorldToCell(new Vector3(0, mainMapBounds.WorldBounds.minY, 0)).y, backgroundOneTilemap.WorldToCell(new Vector3(0, mainMapBounds.WorldBounds.maxY, 0)).y);
        backgroundMapBounds.CellBounds.SetX(backgroundOneTilemap.WorldToCell(new Vector3(mainMapBounds.WorldBounds.minX, 0, 0)).x, backgroundOneTilemap.WorldToCell(new Vector3(mainMapBounds.WorldBounds.maxX, 0, 0)).x);
        backgroundMapBounds.WorldBounds.SetY(mainMapBounds.WorldBounds.minY, mainMapBounds.WorldBounds.maxY);
        backgroundMapBounds.WorldBounds.SetY(mainMapBounds.WorldBounds.minX, mainMapBounds.WorldBounds.maxX);

        //========================================================================================

    }
    private void Update()
    {
        if (generate)
        {
            generate = false;
            hillSeed = rand.Next(1, 2000);

            clearMap();
            DrawGround(currentPreset);
            GenerateHills();

            GenerateLeaves();
            GenerateForeground();
            generateBackground();
            generateGodRays();
        }

    }

    public void clearMap()
    {
        groundTilemapOne.ClearAllTiles();
        groundTilemapTwo.ClearAllTiles();
        decorTilemapOne.ClearAllTiles();
        decorTilemapTwo.ClearAllTiles();
        foregroundTilemap.ClearAllTiles();
        backgroundTwoTilemap.ClearAllTiles();
        backgroundOneTilemap.ClearAllTiles();
        for (int i = 0; i < allGodRays.Count; i++)
        {
            Destroy(allGodRays[i]);

        }
        allGodRays.Clear();

    }



    public void GenerateExits()
    {

    }
    public void generateGodRays()
    {
        int godRayCount = UnityEngine.Random.Range(currentPreset.godRayCount.x, currentPreset.godRayCount.y);
        for (int i = 0; i < godRayCount; i++)
        {
            float pos = UnityEngine.Random.Range(mainMapBounds.WorldBounds.minX,  mainMapBounds.WorldBounds.maxX);
            int godRayType = UnityEngine.Random.Range(0, currentPreset.godRay.Length  -1);
            allGodRays.Add(Instantiate(currentPreset.godRay[godRayType], new Vector3(pos, 0, 0), Quaternion.identity, groundTilemapOne.transform.parent));

        }

    }
    public void generateBackground()
    {
        for (int x = backgroundMapBounds.CellBounds.minX; x <= backgroundMapBounds.CellBounds.maxX; x++)
        {
            for (int y = backgroundMapBounds.CellBounds.minY; y <= backgroundMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = backgroundOneTilemap.CellToWorld(new Vector3Int(x, y, 0));
                float layerOneHeight = backgroundMapBounds.WorldBounds.minY + getGroundNoise(new Vector2(worldPos.x + hillSeed * 3, worldPos.y)) + currentPreset.hillHeight - Mathf.Abs(x) * currentPreset.edgeHillFalloff + currentPreset.backgroundHillHeight;
                if(worldPos.y < layerOneHeight)
                {
                    Plot(new Vector2Int(x, y), backgroundOneTilemap, currentPreset.baseTile);
                }
                if(worldPos.y < layerOneHeight + currentPreset.secondaryBackgroundHillDifference + getGroundNoise(new Vector2(worldPos.x + hillSeed * 4, worldPos.y)))
                {
                    Plot(new Vector2Int(x, y), backgroundTwoTilemap, currentPreset.baseTile);

                }
            }
        }


    }


    public void GenerateLeaves()
    {
                  Decoration temp = currentDecorPreset.Decorations.Find((x) => x.D_Type == DecorationPreset.DecorationType.Leaf);
        if(temp == null) return;

        for (int x = mainMapBounds.CellBounds.minX; x <= mainMapBounds.CellBounds.maxX; x++)
        {
            for (int y = mainMapBounds.CellBounds.minY; y <= mainMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = groundTilemapOne.CellToWorld(new Vector3Int(x, y, 0));
                float leafHeight = mainMapBounds.WorldBounds.maxY - currentPreset.leafDepth + (Mathf.Pow(x, 2) * currentPreset.leafEdgeFalloff) + getLeafNoise(new Vector2(worldPos.x + hillSeed, 0));

                if (worldPos.y > leafHeight)
                {
                    int roll = UnityEngine.Random.Range(0, 100);
                    if (roll < currentPreset.leafDensity)
                    {
                        Plot(new Vector2Int(x, y), decorTilemapOne, temp.D_Tile);
                    }
                }
                if (worldPos.y > leafHeight - currentPreset.leafLayerTwoDifference)
                {
                    int roll = UnityEngine.Random.Range(0, 100);
                    if (roll < currentPreset.leafDensity)
                    {
                        Plot(new Vector2Int(x, y), decorTilemapTwo, temp.D_Tile);
                    }
                }
            }
        }
        for (int x = backgroundMapBounds.CellBounds.minX; x < backgroundMapBounds.CellBounds.maxX; x++)
        {
            for (int y = backgroundMapBounds.CellBounds.minY; y < backgroundMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = backgroundOneTilemap.CellToWorld(new Vector3Int(x, y, 0));

                float leafHeight = backgroundMapBounds.WorldBounds.maxY - currentPreset.backgroundLeafDepth + (Mathf.Pow(x, 2) * currentPreset.backgroundLeafFalloff) + getLeafNoise(new Vector2(worldPos.x + hillSeed, 0));
                if(worldPos.y > leafHeight)
                {
                    Plot(new Vector2Int(x, y), backgroundOneTilemap, temp.D_Tile);

                }
                if (worldPos.y > leafHeight - currentPreset.secondaryBackgroundLeafDepth)
                {
                    Plot(new Vector2Int(x, y), backgroundTwoTilemap, temp.D_Tile);

                }
            }
        }

        for (int x = foregroundMapBounds.CellBounds.minX; x <= foregroundMapBounds.CellBounds.maxX; x++)
        {
            for (int y = foregroundMapBounds.CellBounds.minY; y <= foregroundMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = foregroundTilemap.CellToWorld(new Vector3Int(x, y, 0));
                float leafHeight = foregroundMapBounds.WorldBounds.maxY - currentPreset.leafDepth + (Mathf.Pow(x, 2) * currentPreset.leafEdgeFalloff) + getLeafNoise(new Vector2(worldPos.x + hillSeed, 0));

                if (worldPos.y > leafHeight + currentPreset.leafLayerTwoDifference/2)
                {
                    Plot(new Vector2Int(x, y), foregroundTilemap, temp.D_Tile);
                }
            }
        }


    }

    public void GenerateForeground()
    {
        Decoration leafDecor = currentDecorPreset.Decorations.Find((x) => x.D_Type == DecorationPreset.DecorationType.Leaf);

        for (int x = foregroundMapBounds.CellBounds.minX; x <= foregroundMapBounds.CellBounds.maxX; x++)
        {
            for (int y = foregroundMapBounds.CellBounds.minY; y <= foregroundMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = foregroundTilemap.CellToWorld(new Vector3Int(x, y, 0));


                if (worldPos.y < currentPreset.foreGroundHeight + foregroundMapBounds.WorldBounds.minY - MathF.Abs(x) * currentPreset.foreGroundEdgeFalloff + getForegroundNoise(new Vector2(worldPos.x + hillSeed, 0)))
                {
                    int roll = UnityEngine.Random.Range(50, 150);
                    roll -= (int)MathF.Abs(x) * 10;
                    if(roll < currentPreset.foreGroundLeafDensity)
                    {

                    Plot(new Vector2Int(x, y), foregroundTilemap, leafDecor.D_Tile);
                    }
                    else
                    {
                        Plot(new Vector2Int(x, y), foregroundTilemap, currentPreset.baseTile);

                    }

                }
            }
        }


    }


    public void GenerateHills()
    {
        for (int x = mainMapBounds.CellBounds.minX; x <= mainMapBounds.CellBounds.maxX; x++)
        {
            for (int sy = mainMapBounds.CellBounds.minY; sy <= mainMapBounds.CellBounds.maxY; sy++)
            {



                Vector2 worldPos = groundTilemapOne.CellToWorld(new Vector3Int(x, sy, 0));
                float layerOneHeight = mainMapBounds.WorldBounds.minY + getGroundNoise(new Vector2(worldPos.x + hillSeed, worldPos.y)) + currentPreset.hillHeight - Mathf.Abs(x) * currentPreset.edgeHillFalloff;
                Vector2Int cellPos = new Vector2Int(x, sy);




                if(sy < currentPreset.groundMinHeight - currentPreset.size.y)
                {
                    Plot(cellPos, groundTilemapOne, currentPreset.baseTile);
                    Plot(cellPos, groundTilemapTwo, currentPreset.baseTile);
                }
                if (worldPos.y < layerOneHeight)
                {
                    Plot(cellPos, groundTilemapOne, currentPreset.baseTile);
                    Plot(new Vector2Int(x, sy), groundTilemapTwo, currentPreset.baseTile);

                }
                if (worldPos.y <  layerOneHeight + currentPreset.layerTwoDifference + getGroundNoise(new Vector2(worldPos.x + hillSeed * 2, worldPos.y)))
                {
                    Plot(cellPos, groundTilemapTwo, currentPreset.baseTile);

                }
            }
        }
    }
    public float getGroundNoise(Vector2 _pos)
    {
       return noise.snoise(_pos/currentPreset.hillFrequency) * currentPreset.hillMagnitude + currentPreset.hillHeight;
    }
    public float getForegroundNoise(Vector2 _pos)
    {
        return noise.snoise(_pos / currentPreset.hillFrequency) * currentPreset.foreGroundMagnitude;
    }
    public float getLeafNoise(Vector2 _pos)
    {
        return noise.snoise(_pos / currentPreset.leafFrequency) * currentPreset.leafMagnitude;

    }

    public void DrawGround(RoomPreset _preset)
    {
        Vector2Int Offset = _preset.GetCenter();
        Vector2Int[] Corners = new Vector2Int[4];

        for (int i = 0; i < 4; i++)
        {
            Corners[i] = (_preset.size * CornerMap[i]);
        }
        for (int i = 0; i < Corners.Length; i++)
        {
            if (i < Corners.Length - 1)
            {
                DrawLine(Corners[i], Corners[i + 1], groundTilemapOne, _preset.baseTile);
            }
            else
            {
                DrawLine(Corners[i], Corners[0], groundTilemapOne, _preset.baseTile);
            }
        }

    }





    public static void DrawLine(Vector2Int _pos0, Vector2Int _pos1, Tilemap _TM, TileBase _tile)
    {

        int dx = Math.Abs(_pos1.x - _pos0.x);
        int sx = _pos0.x < _pos1.x ? 1 : -1;
        int dy = -Math.Abs(_pos1.y - _pos0.y);
        int sy = _pos0.y < _pos1.y ? 1 : -1;
        int error = dx + dy;

        while (true)
        {
            Plot(new Vector2Int(_pos0.x, _pos0.y), _TM, _tile);
            if (_pos0.x == _pos1.x && _pos0.y == _pos1.y)
            {

                break;
            }
            int e2 = error * 2;
            if (e2 >= dy)
            {
                if (_pos0.x == _pos1.x) break;


                error = error + dy;
                _pos0.x = _pos0.x + sx;
            }
            if (e2 <= dx)
            {
                if (_pos0.y == _pos1.y) break;
                error = error + dx;
                _pos0.y = _pos0.y + sy;

            }

        }
    }
    public static void Plot(Vector2Int _pos1, Tilemap _TM, TileBase _tile)
    {
        _TM.SetTile((Vector3Int)_pos1, _tile);
    }
}
