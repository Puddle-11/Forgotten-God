using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private RoomPreset currentPreset;
    [SerializeField] private LayerValues[] layers;
    public bool debugSize;

    private void Awake()
    {
        

    }
    private void Update()
    {
        if (debugSize)
        {

            Generate();
            debugSize = false;
        }
    }

    private void Generate()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (i >= currentPreset.layerValues.Length) break;
            layers[i].L_bounds = new TBounds(currentPreset.layerValues[i].L_Scale, currentPreset.size.x ,currentPreset.size.y);
            layers[i].L_baseTilemap.transform.localScale = Vector3.one * currentPreset.layerValues[i].L_Scale;
        }
            Debug.Log(layers[0].L_bounds.Scale);
    }
    private void GenerateLayer()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
}
[System.Serializable]
public class LayerValues
{
    public TBounds L_bounds;
    public Tilemap L_baseTilemap;
    public Tilemap L_decorTilemap;
    public Tilemap L_obstacleTilemap;

}