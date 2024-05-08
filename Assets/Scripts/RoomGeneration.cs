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
    [SerializeField] private GameObject backgroundObj;
    public bool generate;


    private int hillSeed;
    private System.Random rand;
    private TileBounds mainMapBounds = new TileBounds();
    private TileBounds foregroundMapBounds = new TileBounds();
    private TileBounds backgroundMapBounds = new TileBounds();
    private List<GameObject> allGodRays = new List<GameObject>();
    private Decoration LeafTile;


    private Vector2Int[] CornerMap = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
    };

    private void Start()
    {
        LeafTile = currentDecorPreset.Decorations.Find((x) => x.D_Type == DecorationPreset.DecorationType.Leaf);
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
            Generate();
            generate = false;
        }
    }


    public void Generate()
    {
        generate = false;
        hillSeed = rand.Next(0, 50000);
        
        clearMap();

        DrawGround(currentPreset);
        /*
        GenerateHills();

        GenerateForeground();
        generateBackground();
        */
        GenerateGround(mainMapBounds, groundTilemapOne, currentPreset.baseTile, currentPreset.hillHeight, currentPreset.edgeHillFalloff, currentPreset.groundMinHeight, rand.Next(0, 50000));
        GenerateGround(mainMapBounds, groundTilemapTwo, currentPreset.baseTile, currentPreset.hillHeight + currentPreset.layerTwoDifference, currentPreset.edgeHillFalloff, currentPreset.groundMinHeight, rand.Next(0, 50000));

        generateGodRays();
        updateColor();
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

  
    private void updateColor()
    {
        foregroundTilemap.color = currentPreset.foregroundColor;

        groundTilemapOne.color = currentPreset.midgroundOneColor;
        decorTilemapOne.color = currentPreset.midgroundOneColor;

        groundTilemapTwo.color = currentPreset.midgroundTwoColor;
        decorTilemapTwo.color = currentPreset.midgroundTwoColor;

        backgroundOneTilemap.color = currentPreset.backgroundOneColor;

        backgroundTwoTilemap.color = currentPreset.backgroundTwoColor;

        backgroundObj.GetComponent<SpriteRenderer>().color = currentPreset.backdropColor;
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
    public void GenerateGround(TileBounds _bounds,Tilemap _tilemap ,TileBase _tile1, TileBase _tile2, float _height, float _falloff, float _minHeight, int _seed)
    {
        for (int x = _bounds.CellBounds.minX; x < _bounds.CellBounds.maxX; x++)
        {
            for (int y = _bounds.CellBounds.minY; y < _bounds.CellBounds.maxY; y++)
            {

                Vector2 worldPos = groundTilemapOne.CellToWorld(new Vector3Int(x, y, 0));
                float layerOneHeight = _bounds.WorldBounds.minY + getGroundNoise(new Vector2(worldPos.x + _seed, worldPos.y)) + _height - Mathf.Abs(x) * _falloff;
                Vector2Int cellPos = new Vector2Int(x, y);
                if (y < _minHeight - _bounds.CellBounds.maxY)
                {
                    Plot(cellPos, _tilemap, _tile1);
                }
                if (worldPos.y < layerOneHeight)
                {
                    Plot(cellPos, _tilemap, _tile1);

                }
            }
        }
    }
    public void GenerateGround(TileBounds _bounds, Tilemap _tilemap, TileBase _tile1, float _height, float _falloff, float _minHeight, int _seed)
    {
        GenerateGround(_bounds, _tilemap, _tile1, _tile1, _height, _falloff, _minHeight, _seed);
    }
    public void GenerateGround(TileBounds _bounds, Tilemap _tilemap, TileBase _tile1, float _height, float _falloff, int _seed)
    {
        GenerateGround(_bounds, _tilemap, _tile1, _tile1, _height, _falloff, 0, _seed);
    }
    public void GenerateGround(TileBounds _bounds, Tilemap _tilemap, TileBase _tile1, TileBase _tile2, float _height, float _falloff, int _seed)
    {
        GenerateGround(_bounds, _tilemap, _tile1, _tile2, _height, _falloff, 0,  _seed);
    }
    public void generateBackground()
    {

        for (int x = backgroundMapBounds.CellBounds.minX; x <= backgroundMapBounds.CellBounds.maxX; x++)
        {
            for (int y = backgroundMapBounds.CellBounds.minY; y <= backgroundMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = backgroundOneTilemap.CellToWorld(new Vector3Int(x, y, 0));
                float leafHeight = backgroundMapBounds.WorldBounds.maxY - currentPreset.backgroundLeafDepth + (Mathf.Pow(x, 2) * currentPreset.backgroundLeafFalloff) + GetNoise(new Vector2(worldPos.x + hillSeed, 0), currentPreset.hillFrequency, currentPreset.hillMagnitude + currentPreset.hillHeight);
                if (worldPos.y > leafHeight)
                {
                    Plot(new Vector2Int(x, y), backgroundOneTilemap, LeafTile.D_Tile);

                }
                if (worldPos.y > leafHeight - currentPreset.secondaryBackgroundLeafDepth)
                {
                    Plot(new Vector2Int(x, y), backgroundTwoTilemap, LeafTile.D_Tile);

                }
                float layerOneHeight = backgroundMapBounds.WorldBounds.minY + GetNoise(new Vector2(worldPos.x + hillSeed * 3, worldPos.y), currentPreset.hillFrequency, currentPreset.hillMagnitude + currentPreset.hillHeight) + currentPreset.hillHeight - Mathf.Abs(x) * currentPreset.edgeHillFalloff + currentPreset.backgroundHillHeight;
                if(worldPos.y < layerOneHeight)
                {
                    Plot(new Vector2Int(x, y), backgroundOneTilemap, currentPreset.baseTile);
                }
                if(worldPos.y < layerOneHeight + currentPreset.secondaryBackgroundHillDifference + GetNoise(new Vector2(worldPos.x + hillSeed * 4, worldPos.y), currentPreset.hillFrequency, currentPreset.hillMagnitude + currentPreset.hillHeight))
                {
                    Plot(new Vector2Int(x, y), backgroundTwoTilemap, currentPreset.baseTile);
                }
            }
        }
    }


    

    public void GenerateForeground()
    {

        for (int x = foregroundMapBounds.CellBounds.minX; x <= foregroundMapBounds.CellBounds.maxX; x++)
        {
            for (int y = foregroundMapBounds.CellBounds.minY; y <= foregroundMapBounds.CellBounds.maxY; y++)
            {
                Vector2 worldPos = foregroundTilemap.CellToWorld(new Vector3Int(x, y, 0));
                float leafHeight = foregroundMapBounds.WorldBounds.maxY - currentPreset.leafDepth + (Mathf.Pow(x, 2) * currentPreset.leafEdgeFalloff) + GetNoise(new Vector2(worldPos.x + hillSeed, 0), currentPreset.leafFrequency, currentPreset.leafMagnitude);
                float groundHeight = currentPreset.foreGroundHeight + foregroundMapBounds.WorldBounds.minY - MathF.Abs(x) * currentPreset.foreGroundEdgeFalloff + GetNoise(new Vector2(worldPos.x + hillSeed, 0), currentPreset.hillFrequency, currentPreset.foreGroundMagnitude);
               
                //Generate Leaves
                if (worldPos.y > leafHeight + currentPreset.leafLayerTwoDifference / 2) 
                    Plot(new Vector2Int(x, y), foregroundTilemap, LeafTile.D_Tile);

                //Generate ground and bushes
                if (worldPos.y < groundHeight)
                {
                    int roll = UnityEngine.Random.Range(50, 150) - (int)MathF.Abs(x) * 10;
                    TileBase cTile = roll < currentPreset.foreGroundLeafDensity ? LeafTile.D_Tile : currentPreset.baseTile;
                    Plot(new Vector2Int(x, y), foregroundTilemap, cTile);
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
                float layerOneHeight = mainMapBounds.WorldBounds.minY + GetNoise(new Vector2(worldPos.x + hillSeed, worldPos.y), currentPreset.hillFrequency, currentPreset.hillMagnitude + currentPreset.hillHeight) + currentPreset.hillHeight - Mathf.Abs(x) * currentPreset.edgeHillFalloff;
                float leafHeight = mainMapBounds.WorldBounds.maxY - currentPreset.leafDepth + (Mathf.Pow(x, 2) * currentPreset.leafEdgeFalloff) + GetNoise(new Vector2(worldPos.x + hillSeed, 0), currentPreset.leafFrequency, currentPreset.leafMagnitude);
                Vector2Int cellPos = new Vector2Int(x, sy);


                    int roll = UnityEngine.Random.Range(0, 100);
                if (worldPos.y > leafHeight)
                {
                    if (roll < currentPreset.leafDensity)
                    {
                        Plot(cellPos, decorTilemapOne, LeafTile.D_Tile);
                    }
                }
                if (worldPos.y > leafHeight - currentPreset.leafLayerTwoDifference)
                {
                    if (roll < currentPreset.leafDensity)
                    {
                        Plot(cellPos, decorTilemapTwo, LeafTile.D_Tile);
                    }
                }


                if (sy < currentPreset.groundMinHeight - currentPreset.size.y)
                {
                    Plot(cellPos, groundTilemapOne, currentPreset.baseTile);
                    Plot(cellPos, groundTilemapTwo, currentPreset.baseTile);
                }
                if (worldPos.y < layerOneHeight)
                {
                    Plot(cellPos, groundTilemapOne, currentPreset.baseTile);
                    Plot(new Vector2Int(x, sy), groundTilemapTwo, currentPreset.baseTile);

                }
                if (worldPos.y <  layerOneHeight + currentPreset.layerTwoDifference + GetNoise(new Vector2(worldPos.x + hillSeed * 2, worldPos.y), currentPreset.hillFrequency, currentPreset.hillMagnitude + currentPreset.hillHeight))
                {
                    Plot(cellPos, groundTilemapTwo, currentPreset.baseTile);

                }
            }
        }
    }
    /*
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
    */
    public float GetNoise(Vector2 _pos, float _frequency, float _magnitude)
    {
        return noise.snoise(_pos / -_frequency) * _magnitude;
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
    public static void Plot(Vector2Int _pos1, Tilemap[] _TM, TileBase _tile)
    {
        for (int i = 0; i < _TM.Length; i++)
        {
            Plot(_pos1, _TM[i], _tile);
        }
    }
}
