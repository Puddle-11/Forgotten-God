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


    [SerializeField] private RoomLayer[] groundLayers;


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
        for (int i = 0; i < groundLayers.Length; i++)
        {

        }
      
        /*
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
        */
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
        
        //clearMap();

        DrawGround(currentPreset);
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (i >= currentPreset.layerValues.Length)
            {
                break;
            }
            //GenerateGround(groundLayers[i].L_Bounds, groundLayers[i].L_MainTilemap, new TileBase[] {currentPreset.baseTile}, currentPreset.layerValues[i].L_height, currentPreset.layerValues[i].L_falloff, currentPreset.layerValues[i].L_magnitude);

        }


        //GenerateHills();
        //GenerateForeground();
        //generateBackground();



        /*
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (i >= currentPreset.layerValues.Length) break;
            if(i == 1 && groundLayers.Length >= 3)
            {
                GenerateGround(mainMapBounds[i], new Tilemap[] { groundLayers[i], groundLayers[i]}, new TileBase[] { currentPreset.baseTile }, currentPreset.hillHeight, currentPreset.edgeHillFalloff, currentPreset.groundMinHeight, rand.Next(0, 2000));
                continue;
            }
            GenerateGround(mainMapBounds[i], new Tilemap[] { groundLayers[i] }, new TileBase[] { currentPreset.baseTile }, currentPreset.hillHeight, currentPreset.edgeHillFalloff, currentPreset.groundMinHeight, rand.Next(0, 2000));
        }
     */
        //GenerateGround(mainMapBounds, new Tilemap[] { groundTilemapOne, groundTilemapTwo}, new TileBase[] { currentPreset.baseTile }, currentPreset.hillHeight, currentPreset.edgeHillFalloff, currentPreset.groundMinHeight, rand.Next(0, 2000));
        //GenerateGround(mainMapBounds, new Tilemap[] { groundTilemapTwo }, new TileBase[] { currentPreset.baseTile }, currentPreset.hillHeight + currentPreset.layerTwoDifference, currentPreset.edgeHillFalloff, currentPreset.groundMinHeight, rand.Next(0, 2000));



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
        /*
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (i >= currentPreset.layerValues.Length)
            {
                groundLayers[i].color = Color.cyan;

                continue;
            }
            groundLayers[i].color = currentPreset.layerValues[i].L_color;
        }

        */
        backgroundObj.GetComponent<SpriteRenderer>().color = currentPreset.backdropColor;
    }
    public void generateGodRays()
    {
        int godRayCount = UnityEngine.Random.Range(currentPreset.godRayCount.x, currentPreset.godRayCount.y);
        for (int i = 0; i < godRayCount; i++)
        {
            float pos = UnityEngine.Random.Range(groundLayers[i].L_Bounds.WorldBounds.minX, groundLayers[i].L_Bounds.WorldBounds.maxX);
            int godRayType = UnityEngine.Random.Range(0, currentPreset.godRay.Length  -1);
            allGodRays.Add(Instantiate(currentPreset.godRay[godRayType], new Vector3(pos, 0, 0), Quaternion.identity, groundTilemapOne.transform.parent));

        }

    }
    public void GenerateGround(TileBounds _bounds, Tilemap[] _tilemaps ,TileBase[] _tiles, float _height, float _falloff, float _minHeight)
    {
        for (int x = _bounds.CellBounds.minX; x < _bounds.CellBounds.maxX; x++)
        {
            for (int y = _bounds.CellBounds.minY; y < _bounds.CellBounds.maxY; y++)
            {

                Vector2 worldPos = groundTilemapOne.CellToWorld(new Vector3Int(x, y, 0));
                //float layerOneHeight = _bounds.WorldBounds.minY + GetNoise(new Vector2(worldPos.x + rand.Next(0, 2000), worldPos.y), currentPreset.hillFrequency, currentPreset.hillMagnitude + currentPreset.hillHeight) + _height - Mathf.Abs(x) * _falloff;
                Vector2Int cellPos = new Vector2Int(x, y);
                TileBase currentTile = _tiles[UnityEngine.Random.Range(0, _tiles.Length)];
                

                if (y < _minHeight - _bounds.CellBounds.maxY)
                {
                    foreach (Tilemap i in _tilemaps)
                    {

                    Plot(cellPos, i, currentTile);
                    }
                }
                /*
                if (worldPos.y < layerOneHeight)
                {
                    foreach (Tilemap i in _tilemaps)
                    {

                    Plot(cellPos, i, currentTile);
                    }

                }
                */
            }
        }
    }
   /*
    public void GenerateGround(TileBounds _bounds, Tilemap[] _tilemaps, TileBase[] _tiles, float _height, float _falloff, float _magnitude)
    {
        GenerateGround(_bounds, _tilemaps, _tiles, _height, _falloff, 0);
    }
    public void GenerateGround(TileBounds _bounds, Tilemap _tilemap, TileBase _tile, float _height, float _falloff, float _magnitude)
    {
        GenerateGround(_bounds, new Tilemap[] { _tilemap }, new TileBase[] { _tile }, _height, _falloff, 0);
    }
    public void GenerateGround(TileBounds _bounds, Tilemap _tilemap, TileBase[] _tiles, float _height, float _falloff, float _magnitude)
    {
        GenerateGround(_bounds, new Tilemap[] { _tilemap }, _tiles, _height, _falloff, 0);
    }
    public void GenerateGround(TileBounds _bounds, Tilemap[] _tilemaps, TileBase _tile, float _height, float _falloff, float _magnitude)
    {
        GenerateGround(_bounds, _tilemaps, new TileBase[] { _tile }, _height, _falloff, 0);
    }
   */
    /*
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
[System.Serializable]
public class RoomLayer
{
    public Tilemap L_MainTilemap;
    public Tilemap L_DecorationTilemap;
    public Tilemap L_ObstacleTilemap;
    public TileBounds L_Bounds;

}


