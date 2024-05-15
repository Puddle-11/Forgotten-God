using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using System;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting.FullSerializer;
using Cinemachine;
public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private RoomPreset currentPreset;
    [SerializeField] private LayerValues[] layers;
    [SerializeField] private PolygonCollider2D cameraConfiner;
    [SerializeField] private CinemachineConfiner2D cinemachineCam;
    private System.Random rand;

    public bool regenerate;
    private void Update()
    {
        if (regenerate)
        {

            Generate();
            regenerate = false;
        }
    }
    private void Awake()
    {

        rand = new System.Random();
    }
    private void Generate()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (i >= currentPreset.layerValues.Length) break;
            layers[i].L_bounds = new TBounds(currentPreset.layerValues[i].L_scale, currentPreset.size.x, currentPreset.size.y);
            if (layers[i].L_baseTilemap != null)
            {
                layers[i].L_baseTilemap.transform.parent.localScale = Vector3.one * currentPreset.layerValues[i].L_scale;

            }
            GenerateLayer(i, rand.Next(0, 2000));
        }
        GenerateGodRays(transform);
        UpdateConfiner(layers[1], cameraConfiner);
    }
    private void UpdateConfiner(LayerValues _layer, PolygonCollider2D _collider)
    {
        
        Vector2 c1 = _layer.L_baseTilemap.CellToWorld(new Vector3Int(_layer.L_bounds.CellX.max, _layer.L_bounds.CellY.max, 0));
        Vector2 c2 = _layer.L_baseTilemap.CellToWorld(new Vector3Int(_layer.L_bounds.CellX.min + 1, _layer.L_bounds.CellY.max, 0));
        Vector2 c3 = _layer.L_baseTilemap.CellToWorld(new Vector3Int(_layer.L_bounds.CellX.min + 1, _layer.L_bounds.CellY.min + 1, 0));
        Vector2 c4 = _layer.L_baseTilemap.CellToWorld(new Vector3Int(_layer.L_bounds.CellX.max, _layer.L_bounds.CellY.min + 1, 0));

        _collider.points = new Vector2[] {c1, c2, c3, c4};
        cinemachineCam.m_BoundingShape2D = null;

        cinemachineCam.m_BoundingShape2D = _collider;
       

    }
    private void UpdateBackdrop()
    {

    }
   
    private void GenerateGodRays(Transform _parent)
    {
        float y = (layers[0].L_bounds.WorldY.max + layers[0].L_bounds.WorldY.min) / 2;
        for (int i = 0; i < UnityEngine.Random.Range(currentPreset.godRayCount.x, currentPreset.godRayCount.y); i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(layers[0].L_bounds.WorldX.min, layers[0].L_bounds.WorldX.max), y, 0);
            Instantiate(currentPreset.godRay[UnityEngine.Random.Range(0, currentPreset.godRay.Length)], pos, quaternion.identity, _parent);
        }


    }
    private void GenerateLayer(int layerIndex, int _seed)
    {
        LayerValues L = layers[layerIndex];
        BiomeLayer LV = currentPreset.layerValues[layerIndex];
        if (L.L_decorTilemap == null)
        {
            L.L_decorTilemap = L.L_baseTilemap;
        }
        if (LV.L_scale <= 2)
        {
            Vector2Int corner1 = new Vector2Int(L.L_bounds.CellX.max, L.L_bounds.CellY.max);
            Vector2Int corner2 = new Vector2Int(L.L_bounds.CellX.min, L.L_bounds.CellY.max);
            Vector2Int corner3 = new Vector2Int(L.L_bounds.CellX.min, L.L_bounds.CellY.min);
            Vector2Int corner4 = new Vector2Int(L.L_bounds.CellX.max, L.L_bounds.CellY.min);

            DrawLine(corner1, corner2, L.L_baseTilemap, currentPreset.baseTile);
            DrawLine(corner2, corner3, L.L_baseTilemap, currentPreset.baseTile);
            DrawLine(corner3, corner4, L.L_baseTilemap, currentPreset.baseTile);
            DrawLine(corner4, corner1, L.L_baseTilemap, currentPreset.baseTile);
        }
        if (layerIndex + 1< currentPreset.layerValues.Length && layerIndex + 1 < layers.Length && LV.L_scale == currentPreset.layerValues[layerIndex + 1].L_scale)
        {
                GenerateGround(L.L_bounds, layers[layerIndex + 1].L_baseTilemap, currentPreset.baseTile, LV.L_height, LV.L_falloff, LV.L_magnitude, LV.L_terrainScale, _seed);
        }
        if(layerIndex == 0) {
            GenerateGround(L.L_bounds, L.L_baseTilemap, new TileBase[] { currentPreset.leafTile, currentPreset.baseTile, currentPreset.baseTile, currentPreset.baseTile, currentPreset.baseTile }, LV.L_height, LV.L_falloff, LV.L_magnitude, LV.L_terrainScale, _seed);

        }
        else
        {

        GenerateGround(L.L_bounds, L.L_baseTilemap, currentPreset.baseTile, LV.L_height, LV.L_falloff, LV.L_magnitude, LV.L_terrainScale, _seed);
        }
       
        GenerateLeaves(L.L_bounds, L.L_decorTilemap, new TileBase[] { currentPreset.leafTile }, LV.L_leafDepth, LV.L_leafFalloff, LV.L_leafMagnitude, LV.L_leafScale, _seed);
    }
    #region GenerateGround Method and Overloads
    public void GenerateGround(TBounds _bounds, Tilemap[] _tilemaps, TileBase[] _tiles, float _height, float _falloff, float _magnitude, float _frequency, int _seed)
    {
   
        for (int x = _bounds.CellX.min; x < _bounds.CellX.max; x++)
        {
            for (int y = _bounds.CellY.min; y < _bounds.CellY.max; y++)
            {
                Vector2 worldPos = _tilemaps[0].CellToWorld(new Vector3Int(x, y, 0));
                float layerOneHeight = _bounds.WorldY.min + GetNoise(new Vector2(worldPos.x + _seed, worldPos.y), _frequency, _magnitude) + _height - Mathf.Abs(x) * _falloff;
                TileBase currentTile = _tiles[UnityEngine.Random.Range(0, _tiles.Length)];
                if (worldPos.y < layerOneHeight)
                {
                    foreach (Tilemap i in _tilemaps)
                    {

                        Plot(new Vector2Int(x, y), i, currentTile);
                    }

                }
            }
        }
    }
    public void GenerateGround(TBounds _bounds, Tilemap[] _tilemaps, TileBase _tile, float _height, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        GenerateGround(_bounds, _tilemaps, new TileBase[] { _tile }, _height, _falloff, _magnitude, _frequency, _seed);
    }
    public void GenerateGround(TBounds _bounds, Tilemap _tilemap, TileBase[] _tile, float _height, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        GenerateGround(_bounds, new Tilemap[] { _tilemap }, _tile, _height, _falloff, _magnitude, _frequency, _seed);

    }
    public void GenerateGround(TBounds _bounds, Tilemap _tilemap, TileBase _tile, float _height, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        GenerateGround(_bounds, new Tilemap[] {_tilemap}, new TileBase[] { _tile }, _height, _falloff, _magnitude, _frequency, _seed);
    }
    #endregion

    #region GenerateLeaves Method and Overloads
    private void GenerateLeaves(TBounds _bounds, Tilemap[] _tilemaps, TileBase[] _tiles, float _depth, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        for (int x = _bounds.CellX.min; x < _bounds.CellX.max; x++)
        {
            for (int y = _bounds.CellY.min; y < _bounds.CellY.max; y++)
            {
                Vector2 worldPos = _tilemaps[0].CellToWorld(new Vector3Int(x, y, 0));
                float layerOneHeight = _bounds.WorldY.max + GetNoise(new Vector2(worldPos.x + _seed, worldPos.y), _frequency, _magnitude) - _depth - Mathf.Abs(x) * _falloff;
                TileBase currentTile = _tiles[UnityEngine.Random.Range(0, _tiles.Length)];
                if (worldPos.y > layerOneHeight)
                {
                    foreach (Tilemap i in _tilemaps)
                    {

                        Plot(new Vector2Int(x, y), i, currentTile);
                    }

                }
            }
        }
    }
    private void GenerateLeaves(TBounds _bounds, Tilemap _tilemap, TileBase _tile, float _depth, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        GenerateLeaves(_bounds, new Tilemap[] { _tilemap }, new TileBase[] { _tile }, _depth, _falloff, _magnitude, _frequency, _seed);
    }
    private void GenerateLeaves(TBounds _bounds, Tilemap[] _tilemaps, TileBase _tile, float _depth, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        GenerateLeaves(_bounds, _tilemaps, new TileBase[] { _tile }, _depth, _falloff, _magnitude, _frequency, _seed);
    }
    private void GenerateLeaves(TBounds _bounds, Tilemap _tilemap, TileBase[] _tiles, float _depth, float _falloff, float _magnitude, float _frequency, int _seed)
    {
        GenerateLeaves(_bounds, new Tilemap[] { _tilemap }, _tiles, _depth, _falloff, _magnitude, _frequency, _seed);
    }
    #endregion
    public float GetNoise(Vector2 _pos, float _frequency, float _magnitude)
    {
        //removed negative, idk if this will break it or not
        return noise.snoise(_pos / _frequency) * _magnitude;
    }
    #region Drawline and Plot
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
            if (_pos0.x == _pos1.x && _pos0.y == _pos1.y) break;
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
    #endregion
}
[System.Serializable]
public class LayerValues
{
    public TBounds L_bounds;
    public Tilemap L_baseTilemap;
    public Tilemap L_decorTilemap;
    public Tilemap L_obstacleTilemap;

}