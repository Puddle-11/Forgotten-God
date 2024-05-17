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
using UnityEngine.Rendering;
public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration LevelGenRef;
    [SerializeField] private RoomPreset currentPreset;
    [SerializeField] private LayerValues[] layers;
    [SerializeField] private PolygonCollider2D cameraConfiner;
    [SerializeField] private CinemachineConfiner2D cinemachineCam;
    [SerializeField] private SpriteRenderer Backdrop;
    [SerializeField] private float backdropScale;
    public GameObject entrance;
    [SerializeField] private GameObject entrancePrefab;
    private System.Random rand;
    public bool regenerate;
    public bool runPathfinder;
    [SerializeField] private int exitLayer;
    [SerializeField] private GameObject exitPrefab1, exitPrefab2, exitPrefab3, exitPrefab4;
    [SerializeField] private GameObject ditherMask;
    private List<GameObject> levelGarbage = new List<GameObject>();
    private void Awake()
    {
        if(LevelGenRef == null)
        {
            LevelGenRef = this;
        }
        else
        {
            Destroy(this);
        }
        rand = new System.Random();
    }
    private void Update()
    {
        if (runPathfinder)
        {
            PathFind(new Vector2Int(0,1));
            runPathfinder = false;
        }
        if (regenerate)
        {
            Clear();
            Generate();
            regenerate = false;
        }
    
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
        UpdateColors(new Vector2Int(4,ColorManager.CMref.size.y - 1));
        UpdateBackdrop(layers[0].L_bounds);
        GenerateExits(exitLayer, currentPreset.maxExits, new Vector2(1, 1f));
        StopCoroutine("finalizeDelay");
        GlobalManager.Player.SetActive(false);
        GlobalManager.Player.GetComponent<PlayerManager>().MoveToEntrance();
        StartCoroutine(finalizeDelay());
    }
    public IEnumerator finalizeDelay()
    {
        yield return new WaitForSeconds(2);
        GlobalManager.Player.SetActive(true);
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
    private void UpdateBackdrop(TBounds _bounds)
    {
        float sizeX = _bounds.WorldX.max - _bounds.WorldX.min;
        float sizeY = _bounds.WorldY.max - _bounds.WorldY.min;
        Backdrop.transform.localScale = new Vector3(sizeX, sizeY, 1/backdropScale) * backdropScale;
        Backdrop.transform.position = this.transform.position;
    }
    private void UpdateColors(Vector2Int startingIndex)
    {
        Color b1 = ColorManager.CMref.GetTextureColor(new Vector2Int(startingIndex.x + 1, startingIndex.y));
        Color b2 = ColorManager.CMref.GetTextureColor(new Vector2Int(startingIndex.x + 2, startingIndex.y));

        for (int x = 0; x < layers.Length; x++)
        {

            Color c = ColorManager.CMref.GetTextureColor(new Vector2Int(startingIndex.x + x, startingIndex.y));

            layers[x].L_baseTilemap.color = c;
            layers[x].L_decorTilemap.color = c;
        }
        Backdrop.color = ColorManager.CMref.GetTextureColor(new Vector2Int(currentPreset.colorTexture.texture.width, startingIndex.y));
    }
    private void GenerateGodRays(Transform _parent)
    {
        float y = (layers[0].L_bounds.WorldY.max + layers[0].L_bounds.WorldY.min) / 2;
        for (int i = 0; i < UnityEngine.Random.Range(currentPreset.godRayCount.x, currentPreset.godRayCount.y); i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(layers[0].L_bounds.WorldX.min, layers[0].L_bounds.WorldX.max), y, 0);
            levelGarbage.Add(Instantiate(currentPreset.godRay[UnityEngine.Random.Range(0, currentPreset.godRay.Length)], pos, quaternion.identity, _parent));
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
    private void Clear()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].L_baseTilemap != null)
            {
            layers[i].L_baseTilemap.ClearAllTiles();
            }
            if (layers[i].L_obstacleTilemap != null)
            {
                layers[i].L_obstacleTilemap.ClearAllTiles();
            }
            if (layers[i].L_decorTilemap != null)
            {
                layers[i].L_decorTilemap.ClearAllTiles();
            }
        }
        for (int i = 0; i < levelGarbage.Count(); i++)
        {
            Destroy(levelGarbage[i]);

        }
        for (int i = 0; i < levelGarbage.Count(); i++)
        {
            Destroy(levelGarbage[i]);
        }
        levelGarbage.Clear();
        levelGarbage.Clear();
        entrance = null;
    }
    private void GenerateExits(int _index, int _exitNum, Vector2 _tileOffset)
    {
                Tilemap TMap = layers[_index].L_baseTilemap;
        List<Vector2Int> poPos = new List<Vector2Int>();
        for (int x = layers[_index].L_bounds.CellX.min + 1; x < layers[_index].L_bounds.CellX.max - 1; x++)
        {
            for (int y = layers[_index].L_bounds.CellY.min + 1; y < layers[_index].L_bounds.CellY.max - 1; y++)
            {
                if (TMap.GetTile(new Vector3Int(x, y + 1, 0)) == null && TMap.GetTile(new Vector3Int(x, y , 0)) == currentPreset.baseTile)
                {
                    poPos.Add(new Vector2Int(x, y));


                
                }
            }
        }
        for (int i = 0; i < _exitNum; i++)
        {
            int posIndex = UnityEngine.Random.Range(0, poPos.Count());
            Vector2 worldPos = TMap.CellToWorld((Vector3Int)poPos[posIndex]) + new Vector3(_tileOffset.x, _tileOffset.y, 0);

            GameObject exit = Instantiate(exitPrefab1, worldPos, quaternion.identity);
                    levelGarbage.Add(exit);
            GameObject mask = Instantiate(ditherMask, worldPos, quaternion.identity, layers[0].L_baseTilemap.transform);
            mask.transform.localScale = mask.transform.localScale / currentPreset.layerValues[0].L_scale;
            mask.GetComponent<FollowObj>().Target = exit.transform;
            levelGarbage.Add(mask);
            poPos.RemoveAt(posIndex);
        }
        int posIndex2 = UnityEngine.Random.Range(0, poPos.Count());
        Vector2 worldPos2 = TMap.CellToWorld((Vector3Int)poPos[posIndex2]) + new Vector3(_tileOffset.x, _tileOffset.y, 0);
        GameObject entranceObj = Instantiate(entrancePrefab,worldPos2 , quaternion.identity);
        entrance = entranceObj;
        levelGarbage.Add(entrance);
        poPos.RemoveAt(posIndex2);
    }
    public void PathFind(Vector2Int _Pos)
    {
        List<Vector2Int> breakpoints = new List<Vector2Int>() { _Pos };  
    }
    private void RunBreakpoint(Vector2Int _Pos, ref List<Vector2Int> breakpoints)
    {

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
                float layerOneHeight = _bounds.WorldY.max + GetNoise(new Vector2(worldPos.x + _seed, worldPos.y), _frequency, _magnitude) - _depth - (Mathf.Abs(x) * _falloff) / currentPreset.size.x;
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